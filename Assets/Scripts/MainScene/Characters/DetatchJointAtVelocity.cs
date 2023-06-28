using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetatchJointAtVelocity : MonoBehaviour
{
    [SerializeField]
    private float _jointBreakVelocity = 0.1f;

    [SerializeField]
    private GameObject _objToDestroy;

    [SerializeField]
    private UnityEvent _actionOnBreak;

    private ConfigurableJoint _joint;
    private Rigidbody _rb;

    private void Awake()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude > _jointBreakVelocity)
        {
            if (_joint != null)
            {
                Destroy(_joint);
            }

            if (_objToDestroy != null)
            {
                Destroy(_objToDestroy);
            }

            if (_actionOnBreak != null)
            {
                _actionOnBreak.Invoke();
            } 
        }
    }
}
