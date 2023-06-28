using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _npcObjToSpawn;

    [SerializeField]
    private List<Transform> _patrolPoints;

    [SerializeField]
    private int _minInclusive;
    [SerializeField]
    private int _maxExclusive;

    private void Start()
    {
        int numToSpawn = Random.Range(_minInclusive, _maxExclusive);

        for (int i = 0; i < numToSpawn; ++i)
        {
            Vector3 spawnPos = Vector3.zero;

            // Move to random position on navmesh to start
            Vector3 randomPoint = new Vector3(Random.value, 0, Random.value) * 3f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + randomPoint, out hit, 1f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }

            GameObject obj = Instantiate(_npcObjToSpawn, spawnPos, Quaternion.identity);
            obj.transform.parent = gameObject.transform;
            NavmeshMovement movement = obj.GetComponentInChildren<NavmeshMovement>();

            // Set up points in movement script
            movement.patrolPoints = _patrolPoints;
        }
    }
}
