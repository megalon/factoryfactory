using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestAssemblyStationManager))]
public class TestAssemblyStationManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TestAssemblyStationManager script = (TestAssemblyStationManager)target;

        if (script == null)
        {
            Debug.LogError("Script is somehow null!");
            return;
        }

        if (GUILayout.Button("Go to next station"))
        {
            Debug.Log("Go to next station");
            script.GoToNextStation();
        }
    }
}
