using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollower : MonoBehaviour
{
    [SerializeField]
    private Transform _navAgentTransform;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}
