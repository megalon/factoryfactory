using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestMainUI : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float _percent = 0;
    private VisualElement _root;
    private ProgressBar _progressBar;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _progressBar = _root.Q<ProgressBar>("ProgressBarLoading");
    }

    // Update is called once per frame
    void Update()
    {
        _progressBar.value = _percent;
    }
}
