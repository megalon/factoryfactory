using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavmeshMovement : MonoBehaviour
{
    [FormerlySerializedAs("_patrolPoints")]
    public List<Transform> patrolPoints;

    private NavMeshAgent _navMeshAgent;
    private int _patrolPointIndex = 0;
    private float _minSpeed = 1.5f;
    private float _maxSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if (_navMeshAgent.isOnNavMesh)
        {
            SetDestinationToRandomPatrolPoint();
        } else
        {
            Debug.Log("Navmesh agent not on navmesh!");
        }

        _navMeshAgent.speed = Random.Range(_minSpeed, _maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetDestinationToRandomPatrolPoint();
        }

        if(_navMeshAgent.remainingDistance <= 0.1f)
        {
            SetDestinationToRandomPatrolPoint();
        }
    }

    private void SetDestinationToRandomPatrolPoint()
    {
        _navMeshAgent.SetDestination(GetRandomPatrolPointPosition());
    }

    private Vector3 GetRandomPatrolPointPosition()
    {
        return patrolPoints[Random.Range(0, patrolPoints.Count)].position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_navMeshAgent == null || _navMeshAgent.path == null || _navMeshAgent.path.corners.Length < 2)
            return;

        for (int i = 0; i < _navMeshAgent.path.corners.Length - 1; ++i)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(_navMeshAgent.path.corners[i], _navMeshAgent.path.corners[i + 1]);
        }
    }
#endif
}
