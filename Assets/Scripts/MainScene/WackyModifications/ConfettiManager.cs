using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    public static ConfettiManager Instance;

    private List<ParticleSystem> _confettiParticles;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _confettiParticles = new List<ParticleSystem>();

        // Loop through all child objects of the current game object
        for (int i = 0; i < transform.childCount; ++i)
        {
            ParticleSystem temp = transform.GetChild(i).GetComponent<ParticleSystem>();
            
            if (temp == null)
            {
                continue;
            }
            
            _confettiParticles.Add(temp);
        }
    }

    public void EnableConfetti()
    {
        foreach(ParticleSystem ps in _confettiParticles)
        {
            ps.Play();
        }
    }

    public void DisableConfetti()
    {
        foreach (ParticleSystem ps in _confettiParticles)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}
