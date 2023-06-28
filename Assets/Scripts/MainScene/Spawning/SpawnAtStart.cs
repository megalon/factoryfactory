using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtStart : MonoBehaviour
{
    [SerializeField]
    private GameObject _objToSpawn;

    [SerializeField]
    private List<Transform> _spawnPoints;

    [SerializeField]
    [Range(1, 100)]
    private int _chanceToSpawn = 50;

    private void Start()
    {
        foreach (Transform t in _spawnPoints)
        {
            if (Random.Range(0, 100) <= _chanceToSpawn)
            {
                Instantiate(_objToSpawn, t);
            }
        }
    }
}
