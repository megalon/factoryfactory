using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMachine))]
public class StateMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StateMachine sm = (StateMachine)target;

        if (GUILayout.Button("End Show Immediately"))
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Must be in play mode to use this!");
                return;
            }

            Debug.LogWarning("Ending Show Immediately!");

            sm.EndShowImmediately();
        }

        if (GUILayout.Button("Abandon Generating Script"))
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Must be in play mode to use this!");
                return;
            }

            Debug.LogWarning("Abandoning Generating Script!");

            MainScriptHandler.Instance.StopGeneratingImmediately();
        }
    }
}
