using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _objectToPool;
    [SerializeField]
    private int _amountToPool;

    private List<GameObject> _pooledObjects;

    void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < _amountToPool; ++i)
        {
            temp = Instantiate(_objectToPool);
            temp.SetActive(false);
            _pooledObjects.Add(temp);
        }
    }

    public GameObject GetObjectFromPool()
    {
        foreach (GameObject gameObject in _pooledObjects)
        {
            if (!gameObject.activeInHierarchy)
            {
                return gameObject;
            }
        }

        return null;
    }
}
