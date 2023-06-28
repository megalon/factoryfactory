using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MainScript : ExitableMonobehaviour
{
    public List<ScriptSection> scriptSections;
    public ScriptSection productNameScriptSection;
    public string script;
    public int numNarrationLines;
    public List<AssemblyStation> assemblyStationsList;
    public TwitchRequest request;
    public int numStepsToGenerate;

    private bool _testUberduckExceptions = false;

    public MainScript()
    {
        scriptSections = new List<ScriptSection>();
        numNarrationLines = 0;
    }

    public void AssembleScriptFromParts()
    {
        for (int i = 0; i < scriptSections.Count; ++i)
        {
            scriptSections[i].SetResultText(StringUtils.FinalScriptCleanup(scriptSections[i].text));
            script += scriptSections[i].text + "\n";
        }
    }

    public async Task GenerateNarrationPartsAsync()
    {
        int loadingBarIncrementSection = 33 / scriptSections.Count;
        foreach (ScriptSection scriptSection in scriptSections)
        {
            Debug.Log($"GenerateNarrationParts for scriptSection:{scriptSection.promptSO.name}");

            List<string> lines = StringUtils.SplitToLines(scriptSection.text).ToList();
            int loadingBarIncrement = loadingBarIncrementSection / lines.Count;

            foreach (string line in lines)
            {
                if (stopGeneratingImmediately)
                {
                    stopGeneratingImmediately = false;
                    throw new AbandonNarrationGenerationException();
                }

                // Ignore line if it is only whitespace.
                if (line.All(c => char.IsWhiteSpace(c)))
                {
                    continue;
                }

                string tempLine = line;

                if (_testUberduckExceptions)
                {
                    tempLine = "Sorry Uberduck, I am testing exception handling for bad words: retardant";
                    _testUberduckExceptions = false;
                }

                Debug.Log($"Generating narration for:{tempLine}");

                NarrationPart narrationPart = new NarrationPart(tempLine);

                AudioClip clip = await UberduckManager.Instance.GenerateAudioClip(tempLine);

                if (clip != null)
                {
                    narrationPart.setUberduckAudioclip(clip);
                    scriptSection.narrationParts.Add(narrationPart);

                    ++numNarrationLines;
                }
                else
                {
                    Debug.LogWarning("Failed to generate clip! Skipping this dialog...");
                }

                LoadingBarManager.Instance.AppendLoadingBarPercent(loadingBarIncrement);
            }
        }

        Debug.Log("++++ Finished generating audio! ++++");
    }

    public async Task GenerateAssemblyStationList()
    {
        assemblyStationsList = await AssemblyStationManager.Instance.GenerateAssemblyStationsList(scriptSections[1]);

        Debug.Log("++++ Finished generating assembly station list! ++++");
    }

    private void OnDestroy()
    {
        // Destroy the audioclips within this script
        foreach (ScriptSection scriptSection in scriptSections)
        {
            foreach (NarrationPart narrationPart in scriptSection.narrationParts)
            {
                if (narrationPart.audioClipUberduck != null)
                {
                    Destroy(narrationPart.audioClipUberduck);
                }
            }
        }
    }
}
