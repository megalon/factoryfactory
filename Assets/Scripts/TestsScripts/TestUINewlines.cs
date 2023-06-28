using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUINewlines : MonoBehaviour
{
    TextField textFieldNewlineTest;
    Button buttonLogToConsole;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        textFieldNewlineTest = root.Q<TextField>("TextFieldNewlineTest");
        buttonLogToConsole = root.Q<Button>("ButtonLogToConsole");

        buttonLogToConsole.clicked += () => Debug.Log(textFieldNewlineTest.value);
    }
}
