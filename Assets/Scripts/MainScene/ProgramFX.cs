using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramFX : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _meshRenderer.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _meshRenderer.enabled = false;
    }
}
