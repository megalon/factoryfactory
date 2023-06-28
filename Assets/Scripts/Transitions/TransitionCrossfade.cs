using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TransitionCrossfade : MonoBehaviour
{
    public static TransitionCrossfade Instance;

    [SerializeField]
    private int transitionTimeMs = 1000;
    
    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void StartCrossfade()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Camera1"))
        {
            _animator.SetTrigger("SwitchToCamera2");
        } else
        {
            _animator.SetTrigger("SwitchToCamera1");
        }
    }
}
