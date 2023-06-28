using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestTransitions : MonoBehaviour
{

    private VisualElement _root;
    private Button _buttonFadeIn;
    private Button _buttonFadeOut;
    private Button _buttonCrossfade;

    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _buttonFadeIn = _root.Q<Button>("ButtonFadeIn");
        _buttonFadeOut = _root.Q<Button>("ButtonFadeOut");
        _buttonCrossfade = _root.Q<Button>("ButtonCrossfade");

        _buttonFadeIn.clicked += () => TransitionFadeBlack.Instance.FadeFromBlack();
        _buttonFadeOut.clicked += () => TransitionFadeBlack.Instance.FadeToBlack(); ;
        _buttonCrossfade.clicked += () => TransitionCrossfade.Instance.StartCrossfade();
    }
}
