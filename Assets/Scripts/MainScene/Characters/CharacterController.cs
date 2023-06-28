using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private string _ragdollParameterName;

    public void Ragdoll()
    {
        _animator.SetBool(_ragdollParameterName, true);
    }
}
