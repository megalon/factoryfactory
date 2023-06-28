using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnerWithPool))]
public class SpawnerWithPoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnerWithPool script = (SpawnerWithPool)target;

        if (script == null)
        {
            Debug.LogError("Script is somehow null!");
            return;
        }

        if (GUILayout.Button("Spawn"))
        {
            script.Spawn();
        }
    }
}
