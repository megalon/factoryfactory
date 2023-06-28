using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectQueuePool : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToPool;
    [SerializeField]
    private int _amountToPool;
    [SerializeField]
    private bool _poolOnStart = false;

    private Queue<GameObject> _pooledObjects;

    void Start()
    {
        if (_poolOnStart)
        {
            Init();
        }
    }

    public void SetObjectToSpawn(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Could not set object to spawn! Object is null!");
            return;
        }

        _objectToPool = obj;
        Init();
    }

    public void Init()
    {
        Clear();

        if (_objectToPool == null)
        {
            Debug.LogError("Could not init! _objectToPool is null!");
            return;
        }

        if (_pooledObjects == null)
        {
            _pooledObjects = new Queue<GameObject>();
        }

        GameObject temp;
        for (int i = 0; i < _amountToPool; ++i)
        {
            temp = Instantiate(_objectToPool);
            temp.SetActive(false);
            _pooledObjects.Enqueue(temp);
        }
    }

    // Get the next object from the queue, then add it back to the end
    public GameObject GetObjectFromPool()
    {
        if (_pooledObjects == null)
        {
            Debug.LogError("GetObjectFromPool() Pooled objects is null! Could not GetObjectFromPool!");
            return null;
        }

        if (_pooledObjects.Count <= 0)
        {
            Debug.LogError("GetObjectFromPool() Object pool is empty!");
            return null;
        }

        GameObject obj = _pooledObjects.Dequeue();
        _pooledObjects.Enqueue(obj);

        // Need to set velocity to zero
        // so that it doesn't retain movement it had before being reused
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        return obj;
    }

    public void Clear()
    {
        if (_pooledObjects == null) return;

        while (_pooledObjects.Count > 0)
        {
            Destroy(_pooledObjects.Dequeue());
        }

        _pooledObjects.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}