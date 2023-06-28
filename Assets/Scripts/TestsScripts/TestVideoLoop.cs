using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestVideoLoop : MonoBehaviour
{
    private VisualElement _root;
    private Button _buttonPlayVideo;

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        if (_root != null)
        {
            _buttonPlayVideo = _root.Q<Button>("ButtonPlayVid");
            _buttonPlayVideo.clicked += PlayVideo;
        }
    }

    private void PlayVideo()
    {
        Debug.Log("PlayVideo!");
        VideoManager.Instance.PlayVideo(VideoManager.Videos.INTERMISSION);
    }
}
