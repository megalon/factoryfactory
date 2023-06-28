using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TwitchChannelPointManager))]
public class TwitchChannelPointManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TwitchChannelPointManager script = (TwitchChannelPointManager)target;

        if (script == null)
        {
            Debug.LogError("script is null!");
            return;
        }

        if (!EditorApplication.isPlaying)
        {
            GUILayout.Label("(Must be in Play mode to use controls)");
            GUI.enabled = false;
        }

        if (GUILayout.Button("Low Gravity"))
        {
            script.TestLowGravity();
        }

        if (GUILayout.Button("Max Conveyerbelt Speed"))
        {
            script.TestMaxConveyerbeltSpeed();
        }

        if (GUILayout.Button("Small Boxes"))
        {
            script.TestSmallBoxes();
        }

        if (GUILayout.Button("Put Me In Coach!"))
        {
            script.TestPutNPCOnConveyerBelt();
        }

        if (GUILayout.Button("Voice Change"))
        {
            script.TestVoiceChangeRedeem();
        }

        if (GUILayout.Button("Start Redeems"))
        {
            script.TestStartRedeemsForUser();
        }

        if (GUILayout.Button("Stop Redeems"))
        {
            script.TestStopRedeemsForUser();
        }

        if (TimedRedeemManager.Instance != null)
        {
            bool tickClockAllTheTime = GUILayout.Toggle(TimedRedeemManager.Instance.tickClockAllTheTime, "Tick Clock");
            TimedRedeemManager.Instance.tickClockAllTheTime = tickClockAllTheTime;
        } else
        {
            GUILayout.Toggle(false, "Tick Clock");
        }
    }
}
