using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
#if UNITY_EDITOR

    private const float HandleDistanceThreshold = 10f;
    private const float AxisLength = 0.5f;

    private float _distanceToCamera;

    private void OnDrawGizmos()
    {
        _distanceToCamera = Vector3.Distance(transform.position, SceneView.lastActiveSceneView.camera.transform.position);

        if (_distanceToCamera < HandleDistanceThreshold)
        {
            // X axis
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right * AxisLength);

            // Y axis
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.up * AxisLength);

            // Z axis
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.forward * AxisLength);

            // Cube
            Gizmos.color = Color.white;
            Gizmos.DrawCube(transform.position, Vector3.one * AxisLength * 0.25f);

            // Unity always draws this underneath the gizmos for some reason...
            // Label
            //Handles.color = Color.white;
            //var view = UnityEditor.SceneView.currentDrawingSceneView;
            //Handles.Label(transform.position - view.camera.transform.forward, gameObject.name);
        }
    }
#endif
}