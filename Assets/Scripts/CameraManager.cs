using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Transform _startupCamTransform;
    [SerializeField]
    private Transform _intermissionCamTransform;
    [SerializeField]
    private Transform _introCamTransform;
    [SerializeField]
    private Transform _mainCamTransform;
    [SerializeField]
    private Transform _outroCamTransform;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void MoveCameraToAssemblyStation(AssemblyStation assemblyStation)
    {
        _mainCamera.transform.SetParent(null);
        MoveCameraToTransform(assemblyStation.GetCameraPosition());
    }

    public void StateChanged(StateMachine.StateIDs state)
    {
        // Clear camera parent
        _mainCamera.transform.SetParent(null);

        switch (state)
        {
            case StateMachine.StateIDs.STARTUP:
                MoveCameraToTransform(_startupCamTransform);
                break;
            case StateMachine.StateIDs.INTERMISSION:
                MoveCameraToTransform(_intermissionCamTransform);
                break;
            case StateMachine.StateIDs.INTRO:
                MoveCameraToTransform(_introCamTransform);
                _mainCamera.transform.parent = _introCamTransform;
                break;
            case StateMachine.StateIDs.MAIN:
                MoveCameraToTransform(_mainCamTransform);
                break;
            case StateMachine.StateIDs.OUTRO:
                MoveCameraToTransform(_outroCamTransform);
                break;
            default:
                Debug.LogError("CameraManager: State did not exist! Moving to intermission position...");
                MoveCameraToTransform(_intermissionCamTransform);
                break;
        }
    }

    private void MoveCameraToTransform(Transform t)
    {
        if (t == null)
        {
            Debug.LogError("Transform is null in MoveCameraToTransform!");
            return;
        }

        _mainCamera.transform.SetPositionAndRotation(t.position, t.rotation);
    }
}
