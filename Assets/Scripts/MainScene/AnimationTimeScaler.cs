using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimeScaler : MonoBehaviour
{
    [SerializeField]
    private float _scale;

    public List<Animator> animators;

    private float[] _originalSpeeds;

    private void Awake()
    {
        _originalSpeeds = new float[animators.Count];
        for (int i = 0; i < animators.Count; ++i)
        {
            _originalSpeeds[i] = animators[i].speed;
        }
    }

    private void Start()
    {
        ScaleAnimators();
    }

    public void SetTimeScale(float timeScale)
    {
        if (timeScale == _scale)
            return;

        _scale = timeScale;
        ScaleAnimators();
    }

    private void ScaleAnimators()
    {
        for (int i = 0; i < animators.Count; ++i)
        {
            animators[i].speed = _originalSpeeds[i] * _scale;
        }
    }
}
