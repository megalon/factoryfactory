using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TransitionFadeBlack : MonoBehaviour
{
    public static TransitionFadeBlack Instance;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private int _transitionTimeMs = 1000;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void EnableOverlay()
    {
        _animator.gameObject.SetActive(true);
    }

    public void DisableOverlay()
    {
        _animator.gameObject.SetActive(false);
    }

    public void FadeFromBlack()
    {
        _ = FadeFromBlackAsync();
    }

    public async Task<bool> FadeFromBlackAsync()
    {
        return await FadeBlackAsync(FadeState.IN);
    }

    public void FadeToBlack()
    {
        _ = FadeToBlackAsync();
    }

    public async Task<bool> FadeToBlackAsync()
    {
        return await FadeBlackAsync(FadeState.OUT);
    }

    private async Task<bool> FadeBlackAsync(FadeState crossfadeState)
    {
        switch (crossfadeState)
        {
            case FadeState.IN:
                //Debug.Log("Fading IN!");
                _animator.SetTrigger("FadeIn");
                _animator.ResetTrigger("FadeOut");
                break;
            case FadeState.OUT:
                //Debug.Log("Fading OUT!");
                _animator.SetTrigger("FadeOut");
                _animator.ResetTrigger("FadeIn");
                break;
        }

        await Task.Delay(_transitionTimeMs);
        return true;
    }

    public enum FadeState
    {
        IN,
        OUT
    }
}
