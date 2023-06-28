using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentSpeedToAnimationSpeed : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private string _nameOfAnimFloatParameter;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _animator.SetFloat(_nameOfAnimFloatParameter, _navMeshAgent.velocity.magnitude);
    }
}
