using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimationStart : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    public float percentOffset;
    [SerializeField]
    private string _stateName;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _animator.Play(_stateName, 0, percentOffset / 100);
    }
}
