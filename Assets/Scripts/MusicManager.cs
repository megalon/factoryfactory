using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public float fadeAmount = 0.01f;
    public float targetVolume = 0.4f;

    [SerializeField]
    private AudioClip _intermissionMusic;
    private AudioSource _audioSource;
    private int _pausedTimeSamples = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(StateMachine.StateIDs state)
    {
        PlayMusicAsync(state);
    }

    private async void PlayMusicAsync(StateMachine.StateIDs state)
    {
        if (_audioSource.clip != null)
        {
            await FadeVolumeOut();
        }

        _pausedTimeSamples = _audioSource.timeSamples;
        _audioSource.Pause();
        _audioSource.volume = 0;
        _audioSource.clip = null;
        switch (state)
        {
            case StateMachine.StateIDs.STARTUP:
                _audioSource.clip = _intermissionMusic;
                break;
            case StateMachine.StateIDs.INTERMISSION:
                _audioSource.clip = _intermissionMusic;
                break;
            case StateMachine.StateIDs.INTRO:
                break;
            case StateMachine.StateIDs.MAIN:
                break;
            case StateMachine.StateIDs.OUTRO:
                break;
        }

        if (_audioSource.clip != null)
        {
            _audioSource.timeSamples = _pausedTimeSamples;
            _audioSource.Play();
            await FadeVolumeIn();
        }
    }

    private async Task FadeVolumeIn()
    {
        Debug.Log("Fading in...");
        do
        {
            if (_audioSource.volume + fadeAmount > targetVolume)
            {
                _audioSource.volume = targetVolume;
            }
            else
            {
                _audioSource.volume += fadeAmount;
                await Task.Delay(10);
            }
        } while (_audioSource.volume < targetVolume);
    }

    private async Task FadeVolumeOut()
    {
        Debug.Log("Fading out...");
        do
        {
            if (_audioSource.volume - fadeAmount < 0)
            {
                _audioSource.volume = 0;
            } else
            {
                _audioSource.volume -= fadeAmount;
                await Task.Delay(10);
            }
        } while (_audioSource.volume > 0);
    }
}