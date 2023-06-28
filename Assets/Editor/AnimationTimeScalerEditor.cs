using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationTimeScaler))]
public class AnimationTimeScalerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AnimationTimeScaler script = (AnimationTimeScaler)target;

        if (script == null)
        {
            Debug.LogError("Script is somehow null!");
            return;
        }

        if (GUILayout.Button("Evenly Space Offsets"))
        {
            for (int i = 0; i < script.animators.Count; ++i)
            {
                OffsetAnimationStart offsetAnimation = script.animators[i].GetComponent<OffsetAnimationStart>();

                if (offsetAnimation == null)
                {
                    Debug.LogWarning(offsetAnimation.gameObject.name + " Animator object did not have an OffsetAnimationStart script! Ignoring...");
                    continue;
                }
                offsetAnimation.percentOffset = (100 / script.animators.Count) * i;
            }
        }
    }
}


