using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnOnAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private WorkPartSpawner _spawner;

    public void SpawnEvent()
    {
        _spawner.SpawnFromAnimationEvent();
    }
}
