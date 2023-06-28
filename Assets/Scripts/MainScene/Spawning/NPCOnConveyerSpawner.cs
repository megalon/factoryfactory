using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCOnConveyerSpawner : SpawnerWithPool
{
    private int _transIndex = -1;

    private void Start()
    {
        SetupTransIndex();
    }

    private bool SetupTransIndex()
    {
        AssemblyStation station = GetComponentInParent<AssemblyStation>();
        if (station != null)
        {
            _transIndex = station.GetIndexInAssemblyLine();
            return true;
        }
        Debug.LogError("Could not find parent AssemblyStation in NPCOnConveyerSpawner!");

        return false;
    }

    public void SpawnNPC()
    {
        if (_transIndex < 0)
        {
            if (SetupTransIndex())
            {
                Debug.LogError("Skipping SpawnNPC because _transIndex < 0 !");
                return;
            }
        }

        GameObject obj = Spawn();

        obj.transform.Rotate(new Vector3(Random.Range(0, 4) * 90, 0, 0));

        Transformable transformable = obj.GetComponent<Transformable>();

        if (transformable == null)
        {
            Debug.LogError("No transformable on object " + obj.name);
            return;
        }

        // Hack to get around issue with transformable starting in the wrong phase
        // Init here then init again after adding transformation
        // The transformations are typically added to the gameobject that is then instantiated
        // Here we are not re-instsantiating the object, so it was having issues
        transformable.Init(_transIndex);
        
        AssemblyStationManager.Instance.AddTransformationToObject(obj, _transIndex);
        
        transformable.Init(_transIndex);
    }
}
