using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestPolyPizza))]
public class TestPolyPizzaEditor : Editor
{
    string textInput = "";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestPolyPizza source = (TestPolyPizza)target;

        GUILayout.Label("Search");

        textInput = GUILayout.TextField(textInput);

        if (GUILayout.Button("Download Model"))
        {
            source.GetModelAsync(textInput);
        }
    }
}
