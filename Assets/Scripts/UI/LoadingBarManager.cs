using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingBarManager : MonoBehaviour
{
    public static LoadingBarManager Instance;
    private VisualElement _progressBar;
    private TextElement _progressText;
    private GroupBox _groupBoxLoading;
    private float _percent = 0;
    private float _lerpedPercent = 0;
    [SerializeField]
    private int _fadeTimeSec = 3;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _progressBar = root.Q<VisualElement>("LoadingBar");
        _progressText = root.Q<TextElement>("ProgressText");
        _groupBoxLoading = root.Q<GroupBox>("GroupBoxLoading");

        // Hide at start
        HideLoadingBar();
    }

    public void ShowLoadingBar()
    {
        _groupBoxLoading.visible = true;
        StartCoroutine(FadeIn());
    }

    public void HideLoadingBar()
    {
        _groupBoxLoading.visible = false;
        _groupBoxLoading.style.opacity = new StyleFloat(0f);
    }

    public bool LoadingBarIsVisible()
    {
        return _groupBoxLoading.visible;
    }

    public void SetLoadingBarPercent(float percent)
    {
        if (percent < _lerpedPercent)
        {
            _lerpedPercent = percent;
        }

        Debug.Log("Setting loading bar to:" + percent);
        _percent = percent;
    }

    private void Update()
    {
        if (_lerpedPercent < _percent)
        {
            _lerpedPercent = Mathf.Lerp(_lerpedPercent, _percent, 10 * Time.deltaTime);
            _progressText.text = $"{Mathf.CeilToInt(_lerpedPercent)}%";
            _progressBar.style.width = new StyleLength(Length.Percent(Mathf.Clamp(_lerpedPercent, 0, 100)));
        }
    }

    public float GetLoadingBarPercent()
    {
        return _percent;
    }

    public void AppendLoadingBarPercent(float percentToAdd)
    {
        SetLoadingBarPercent(GetLoadingBarPercent() + percentToAdd);
    }

    private IEnumerator FadeIn()
    {
        for (float i = 0; i < _fadeTimeSec; i += Time.deltaTime)
        {
            _groupBoxLoading.style.opacity = new StyleFloat(i);
            yield return null;
        }
    }
}
