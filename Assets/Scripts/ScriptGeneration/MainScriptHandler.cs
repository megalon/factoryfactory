using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScriptHandler : ExitableMonobehaviour
{
    public static MainScriptHandler Instance;
    public OpenAIScriptBuilderV2 scriptBuilder;

    public float timeBeforeLinesSec = 1;
    public float timeAfterLinesSec = 1;

    public int maxScriptsToCache = 3;

    public GameObject UIGameObject;
    private VisualElement root;
    private TextField textFieldFullScript;
    private AudioSource audioSource;

    public MainScript currentScript { get; private set; }

    private Queue<MainScript> mainScripts;
    private MainScript _scriptBeingGenerated;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        if (UIGameObject != null)
        {
            root = UIGameObject.GetComponent<UIDocument>().rootVisualElement;
            if (root != null)
            {
                textFieldFullScript = root.Q<TextField>("TextFieldFullScript");
            }
        }

        audioSource = GetComponent<AudioSource>();
        mainScripts = new Queue<MainScript>();
    }

    /// <summary>
    /// Generates the script text and narration audio files
    /// </summary>
    public void GenerateNewScript()
    {
        if (mainScripts.Count >= maxScriptsToCache)
        {
            Debug.LogWarning($"Script cache limit hit. {maxScriptsToCache} scripts ready. Not generating a new one.");
            return;
        }

        GenerateNewScriptAsync();
    }

    private async void GenerateNewScriptAsync()
    {
        try
        {
            LoadingBarManager.Instance.SetLoadingBarPercent(0);

            #region Text
            _scriptBeingGenerated = await scriptBuilder.GenerateNewScript();

            if (_scriptBeingGenerated == null)
            {
                Debug.LogError("Got null script from script builder!");

                // What do we do here?
                return;
            }
            #endregion

            if (stopGeneratingImmediately)
            {
                throw new AbandonScriptGenerationException();
            }

            #region Audio
            if (_scriptBeingGenerated.request != null)
            {
                // Get the voice from this request, if specified
                if (_scriptBeingGenerated.request.VoiceID != null && !_scriptBeingGenerated.request.VoiceID.Equals(string.Empty))
                {
                    VoiceChangeManager.Instance.SetNextVoiceId(_scriptBeingGenerated.request.VoiceID, true);
                }
            }

            // Need to switch to the next voice before generating the dialog
            // This could be the user request voice, or voice change by moderators
            VoiceChangeManager.Instance.SwitchToNextVoice();

            await _scriptBeingGenerated.GenerateNarrationPartsAsync();

            #endregion

            if (stopGeneratingImmediately)
            {
                throw new AbandonScriptGenerationException();
            }

            #region Assembly Stations
            await _scriptBeingGenerated.GenerateAssemblyStationList();
            #endregion
        }
        catch (Exception e)
        {
            Debug.LogError("Caught exception in MainScriptHandler when generating new script!");
            Debug.LogError(e.Message);

            Debug.LogWarning("Abandoned script in GenerateNewScriptAsync!");

            ResetStopGeneratingImmediately();

            Destroy(_scriptBeingGenerated);

            // What do we do here?
            // Somehow signal we need to generate another script
            TryToGenerateNewScript();

            return;
        }

        Debug.Log("++++ Finished generating script! Adding to scripts queue. ++++");

        mainScripts.Enqueue(_scriptBeingGenerated);
    }

    private async void TryToGenerateNewScript()
    {
        await Task.Delay(1000);

        Debug.Log("Trying to generate a script again!");
        GenerateNewScript();
    }

    public bool IsNextScriptReady()
    {
        if (mainScripts.Count >= 1)
            return true;
        return false;
    }

    public void NextScript()
    {
        // Destroy old script
        Destroy(currentScript);

        if (mainScripts.Count <= 0)
        {
            Debug.LogError("Cannot go to next script! No scripts in queue!");
            return;
        }

        // Set new script
        currentScript = mainScripts.Dequeue();

        // Add the requester to the timeout dict
        if (currentScript != null && currentScript.request != null)
        {
            RequestsManager.Instance.AddRequestToTimeout(currentScript.request);
        }

        SetFullScriptTextField(currentScript);
    }

    public void SetFullScriptTextField(MainScript script)
    {
        if (textFieldFullScript != null)
        {
            textFieldFullScript.value = script.script;
        }
    }

    public async Task<bool> PlayScriptSectionAudioAsync(ScriptSection scriptSection)
    {
        Debug.LogWarning($"Playing {scriptSection.promptSO.name}");
        foreach (NarrationPart narrationPart in scriptSection.narrationParts)
        {
            await PlayNarrationPartAudioAsync(narrationPart);
        }

        return true;
    }

    public async Task<bool> PlayNarrationPartAudioAsync(NarrationPart narrationPart)
    {
        Debug.LogWarning($"Playing line: {narrationPart.text}");
        audioSource.Stop();
        audioSource.clip = narrationPart.audioClipUberduck;

        await Task.Delay((int)(timeBeforeLinesSec * 1000));

        audioSource.Play();
        UIManager.Instance.SubtitleNarration(narrationPart);

        while (audioSource.isPlaying)
        {
            await Task.Delay(100);

            if (exitStateImmediately)
            {
                exitStateImmediately = false;
                audioSource.Stop();
                UIManager.Instance.ExitStateImmediately();
                throw new ExitingStateEarlyException();
            }
        }

        await Task.Delay((int)(timeAfterLinesSec * 1000));

        return true;
    }

    public void IgnoreNextScriptAndGenerateNewOne()
    {
        Debug.LogWarning("Ignoring next script and generating a new one!");
        if (IsNextScriptReady())
        {
            Destroy(mainScripts.Dequeue());
            GenerateNewScript();
        } else
        {
            StopGeneratingImmediately();
        }
    }

    public bool IsNextScriptUserRequest()
    {
        return scriptBuilder.IsNextScriptUserRequest;
    }
}
