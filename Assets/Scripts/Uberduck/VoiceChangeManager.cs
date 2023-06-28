using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class VoiceChangeManager : MonoBehaviour
{
    public static VoiceChangeManager Instance;

    [SerializeField]
    private string _defaultVoice;
    [SerializeField]
    private float _defaultVolume;
    private string _nextVoiceID = "";

    public string DefaultVoice { get => _defaultVoice; }
    public float DefaultVolume { get => _defaultVolume; }

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
    }

    public void SwitchToNextVoice()
    {
        if (_nextVoiceID.Equals(string.Empty))
        {
            UberduckManager.Instance.voiceName = DefaultVoice;
            return;
        }

        UberduckManager.Instance.voiceName = _nextVoiceID;
        _nextVoiceID = string.Empty;
    }

    public void SetNextVoiceId(string nextVoiceId, bool assumeExists = false)
    {
        if (assumeExists)
        {
            _nextVoiceID = nextVoiceId;
            return;
        }

        _ = SetNextVoiceIdAsync(nextVoiceId);
    }

    public async Task<bool> SetDefaultVoice(string voice)
    {
        if (await UberduckManager.Instance.CheckIfVoiceExists(voice))
        {
            _defaultVoice = voice;
            return true;
        }

        Debug.LogWarning($"Voice {_nextVoiceID} did not exist on Uberduck API!");
        return false;
    }

    public async Task<bool> SetNextVoiceIdAsync(string nextVoiceId)
    {
        if (await UberduckManager.Instance.CheckIfVoiceExists(nextVoiceId))
        {
            _nextVoiceID = nextVoiceId;
            return true;
        }

        Debug.LogWarning($"Voice {_nextVoiceID} did not exist on Uberduck API!");
        return false;
    }

    public void SetVolumeForCurrentVoice()
    {
        if (MainScriptHandler.Instance.currentScript.request != null)
        {
            SetVolumeForVoiceID(MainScriptHandler.Instance.currentScript.request.VoiceID);
        }
        else
        {
            SetVolumeForVoiceID(DefaultVoice);
        }
    }

    private void SetVolumeForVoiceID(string voiceID)
    {
        _audioSource.volume = GetVolumeForVoiceID(voiceID);
    }

    private float GetVolumeForVoiceID(string voiceID)
    {
        if (TwitchChannelPointManager.Instance == null)
        {
            Debug.LogWarning("TwitchChannelPointManager Instance is null! Ignoring VoiceID volume...");
            return DefaultVolume;
        }

        // Find the voiceID then get the volume
        foreach ((string, float) tuple in TwitchChannelPointManager.Instance._voicesDict.Values)
        {
            if (tuple.Item1.Equals(voiceID))
            {
                return tuple.Item2;
            }
        }

        return DefaultVolume;
    }
}