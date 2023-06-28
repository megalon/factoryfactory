using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WackyModificationsManager : MonoBehaviour
{
    public static WackyModificationsManager Instance;
    public static Action WorkPartScaleChanged;

    private Vector3 _defaultGravity;
    private bool _isMaxConveyerSpeed = false;
    private Vector3 _wackyModificationScale;

    public bool IsMaxConveyerSpeed { get => _isMaxConveyerSpeed; }
    public Vector3 WackyModificationScale { get => _wackyModificationScale; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _defaultGravity = Physics.gravity;
        _wackyModificationScale = Vector3.one;
    }

    public void SetGravity(Vector3 gravityIn)
    {
        Debug.Log("Setting gravity to: " + gravityIn.ToString());
        Physics.gravity = gravityIn;
    }

    public void ResetGravityToDefault()
    {
        Physics.gravity = _defaultGravity;
    }

    public void SetMaxConveyerbeltSpeed()
    {
        _isMaxConveyerSpeed = true;
        AssemblyStationManager.Instance.SetConveyerBeltSpeed(8f);
    }

    public void SetRandomConveyerbeltSpeed()
    {
        _isMaxConveyerSpeed = false;
        
        Debug.Log("SetRandomConveyerbeltSpeed");

        // 1.3f is default conveyerbelt speed
        float conveyerbeltSpeed = 1.3f;
        float rand = UnityEngine.Random.Range(0, 100);

        if (rand >= 20)
        {
            conveyerbeltSpeed = UnityEngine.Random.Range(1.5f, 4f);
        }

        AssemblyStationManager.Instance.SetConveyerBeltSpeed(conveyerbeltSpeed);
    }

    public void SetWorkPartScale(Vector3 scale)
    {
        Debug.Log("SetWorkPartScale: " + scale.ToString());
        _wackyModificationScale = scale;

        try
        {
            WorkPartScaleChanged();
        } catch (Exception e)
        {
            if (e != null)
                Debug.LogError(e.Message);
            else
                Debug.LogError("Unspecified error in WorkPartScaleChanged!");
        }
    }
}
