using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWithPool : MonoBehaviour
{
    [SerializeField]
    protected float _spawnIntervalSec;

    [SerializeField]
    protected Vector3 _spawnVelocity;

    protected float _timeRemaining;

    protected ObjectQueuePool _pool;

    private void Awake()
    {
        _pool = GetComponent<ObjectQueuePool>();
    }

    public void SetObjectToSpawn(GameObject obj)
    {
        _pool.SetObjectToSpawn(obj);
    }

    public GameObject Spawn()
    {
        GameObject obj = _pool.GetObjectFromPool();

        if (obj == null)
        {
            Debug.LogError("Could not spawn object! Pool is empty!");
            return null;
        }

        obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        obj.SetActive(true);

        if (_spawnVelocity != Vector3.zero)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = _spawnVelocity;
            }
        }

        return obj;
    }
}
