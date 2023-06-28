using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkPartSpawner : SpawnerWithPool
{
    [SerializeField]
    private bool _spawnAutomatically = true;
    private int _transIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (_spawnAutomatically)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
            {
                _timeRemaining = _spawnIntervalSec;
                SpawnAndEnableTransformations();
            }
        }
    }

    public void SetTransformationIndex(int transIndex)
    {
        _transIndex = transIndex;
    } 

    public void SpawnFromAnimationEvent()
    {
        SpawnAndEnableTransformations();
    }

    private void SpawnAndEnableTransformations()
    {
        GameObject obj = Spawn();

        if (obj == null)
        {
            Debug.LogWarning("SpawnAndEnableTransformations() Could not spawn object! obj is null!");
            return;
        }

        Transformable wp = obj.GetComponent<Transformable>();

        if (wp == null)
        {
            Debug.Log("Object to spawn did not have Transformable script! Ignoring Transformable...");
            return;
        }

        wp.Init(_transIndex);
    }
}
