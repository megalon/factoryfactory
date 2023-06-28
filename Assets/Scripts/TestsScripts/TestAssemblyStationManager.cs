using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestAssemblyStationManager : MonoBehaviour
{
    private AssemblyStationManager _stationManager;

    private void Awake()
    {
        _stationManager = GetComponent<AssemblyStationManager>();
    }

    private void Start()
    {
        _stationManager.InitTestStations();
        CameraManager.Instance.MoveCameraToAssemblyStation(_stationManager.GetCurrentStation());
    }

    public void GoToNextStation()
    {
        _stationManager.MoveToNextAssemblyStation();
    }
}