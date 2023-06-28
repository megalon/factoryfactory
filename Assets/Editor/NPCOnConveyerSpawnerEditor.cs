using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPCOnConveyerSpawner))]
public class NPCOnConveyerSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NPCOnConveyerSpawner script = (NPCOnConveyerSpawner)target;

        if (script == null)
        {
            Debug.LogError("Script is somehow null!");
            return;
        }

        if (GUILayout.Button("Spawn"))
        {
            script.SpawnNPC();
        }
    }

}
