using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : ExitableMonobehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private float _scrollSpeed = 30f;
    private Vector2 _productTextSize;

    private VisualElement _root;
    private VisualElement _productNameParent;
    private Label _labelProductName;
    private Label _labelSubtitles;
    private Label _labelRequesterName;
    private Scroller _scroller;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _root = GetComponent<UIDocument>().rootVisualElement;
        _productNameParent = _root.Q<VisualElement>("ProductNameParent");
        _labelProductName = _root.Q<Label>("LabelProductName");
        _labelSubtitles = _root.Q<Label>("LabelSubtitles");
        _labelRequesterName = _root.Q<Label>("LabelRequesterName");
        ScrollView scrollView = _root.Q<ScrollView>("ScrollView");
        _scroller = scrollView.verticalScroller;

        ClearUI();
    }

    private void Update()
    {
        // Scroll product name if text is long enough
        if (_productTextSize.x > _root.resolvedStyle.width)
        {
            Vector3 labelPos = _labelProductName.transform.position;
            labelPos.x -= _scrollSpeed * Time.deltaTime;
            if (labelPos.x < - (_productTextSize.x))
            {
                labelPos.x = 0;
            }
            _labelProductName.transform.position = labelPos;
        } else
        {
            if (_labelProductName.transform.position.x != 0)
            {
                _labelProductName.transform.position = Vector3.zero;
            }
        }
    }

    private void LateUpdate()
    {
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        _scroller.value = _scroller.highValue > 0 ? _scroller.highValue : 0;
    }

    public void ClearUI()
    {
        SetProductName("");
        SetSubtitles("");
        SetRequesterName("");
    }

    public void SetScriptInfoInUI()
    {
        string requesterName = "";

        if (MainScriptHandler.Instance.currentScript.request != null)
        {
            if (MainScriptHandler.Instance.currentScript.request.User != null)
            {
                requesterName = MainScriptHandler.Instance.currentScript.request.User.DisplayName;
            }
        }

        SetRequesterName(requesterName);

        SetProductName(MainScriptHandler.Instance.currentScript.productNameScriptSection.text);

    }

    public void SetProductName(string text)
    {
        SetLabelText(_labelProductName, text);
        CalculateTextSize(_labelProductName.text);

        if (_productTextSize.x > _root.resolvedStyle.width)
        {
            string spacer = "      ";
            
            SetLabelText(_labelProductName, text + spacer + text);

            // We need to calculate the width of the text with a spacer at the end,
            // but "MeasureTextSize" will strip whitespace, so we have to do this workaround

            // Calculate the text size with the . at the end because trailing whitespace gets ignored without it
            CalculateTextSize(text + spacer + ".");

            // Need to get the length of our terminator character
            Vector2 periodSize = _labelProductName.MeasureTextSize(".", 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined);

            // Remove the length of the terminator characater we added at the end
            // Now the _productTextSize includes the length of the whitespace
            _productTextSize = _productTextSize - periodSize;
        }

        if (_labelProductName.text == null || _labelProductName.text.Equals(string.Empty))
        {
            _productNameParent.visible = false;
        } else
        {
            _productNameParent.visible = true;
        }
    }

    private void CalculateTextSize(string text)
    {
        _productTextSize = _labelProductName.MeasureTextSize(text, 0, VisualElement.MeasureMode.Undefined, 0, VisualElement.MeasureMode.Undefined);
    }

    public void SetSubtitles(string text)
    {
        SetLabelText(_labelSubtitles, text);
    }

    public void SetRequesterName(string text)
    {
        if (text.Equals(string.Empty))
        {
            _labelRequesterName.visible = false;
        }
        else
        {
            _labelRequesterName.visible = true;
            SetLabelText(_labelRequesterName, text);
        }
    }

    private void AppendToLabel(Label label, string text)
    {
        SetLabelText(label, label.text + text);
    }

    public void SubtitleNarration(NarrationPart narrationPart)
    {
        SubtitleNarrationAsync(narrationPart);
    }

    private async void SubtitleNarrationAsync(NarrationPart narrationPart)
    {
        string[] subtitleParts = Regex.Split(narrationPart.text, @"\s+");

        int timePerSubtitleMS = (int)((narrationPart.audioClipUberduck.length / subtitleParts.Length) * 1000);

        SetSubtitles("");

        try
        {
            foreach (string subtitle in subtitleParts)
            {
                AppendToLabel(_labelSubtitles, " " + subtitle);
                ScrollToBottom();
                await Task.Delay(timePerSubtitleMS);

                if (exitStateImmediately)
                {
                    exitStateImmediately = false;
                    throw new ExitingStateEarlyException();
                }
            }
        } catch (ExitingStateEarlyException)
        {
            Debug.LogWarning("Got ExitingStateEarlyException! Stopping subtitles...");
        }
    }

    private void SetLabelText(Label label, string text)
    {
        text = text?.Trim();

        if (text == null || text.Equals(""))
        {
            label.text = "";
            label.visible = false;

            return;
        }
        
        if (!label.visible)
        {
            label.visible = true;
        }

        label.text = text;
    }
}
