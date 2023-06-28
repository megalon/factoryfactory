using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreSpecificColliders : MonoBehaviour
{
    [SerializeField]
    private List<Collider> _collidersToIgnore;
    private Collider[] _colliders;

    private void Awake()
    {
        _colliders = GetComponents<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_collidersToIgnore.Contains(collision.collider))
        {
            foreach (Collider collider in _colliders)
            {
                Physics.IgnoreCollision(collider, collision.collider, true);
            }
        }
    }
}
