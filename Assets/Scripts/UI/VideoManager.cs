using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;

    [SerializeField]
    private GameObject _regularVideoObj;
    [SerializeField]
    private GameObject _intermissionLoopObj;
    [SerializeField]
    private VideoClip _intermissionClip;
    private VideoPlayer _videoPlayer;
    private Videos _currentVideo;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _videoPlayer = _regularVideoObj.GetComponent<VideoPlayer>();
        _currentVideo = Videos.INTERMISSION;
    }

    public void PlayVideo(Videos video)
    {
        _currentVideo = video;
        switch (_currentVideo)
        {
            default:
                _videoPlayer.clip = _intermissionClip;
                PlayIntermissionLoopAsync();
                break;
        }

        if (_videoPlayer.clip == null)
        {
            return;
        }

        _videoPlayer.Play();
    }

    public async void PlayIntermissionLoopAsync()
    {
        await Task.Delay(3000);
        if (_videoPlayer.isPlaying)
        {
            _intermissionLoopObj.SetActive(true);
            _currentVideo = Videos.INTERMISSION_LOOP;
        }
    }


    public void StopVideo()
    {
        _videoPlayer.Stop();
        _intermissionLoopObj.SetActive(false);
    }

    public enum Videos
    {
        INTERMISSION,
        INTERMISSION_LOOP
    }
}
