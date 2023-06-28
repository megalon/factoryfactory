using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This class exists so that we can keep track of all of the NPCs we need to spawn,
/// and delay when they are spawned (so they don't all spawn at the same time inside of eachother)
/// </summary>
public class NPCRedeemsManager : MonoBehaviour
{
    public static NPCRedeemsManager Instance;

    [SerializeField]
    private int _spawnDelayTimeMS = 250;
    [SerializeField, Tooltip("Max NPCs To Spawn")]
    private int _maxNPCsToSpawn = 10;

    private int _numNPCsToSpawn = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    private void Start()
    {
        MainSceneActions.OnNextAssemblyStation += SpawnNPCs;
    }

    private void OnDestroy()
    {
        MainSceneActions.OnNextAssemblyStation -= SpawnNPCs;
    }

    private void SpawnNPCs()
    {
        if (_numNPCsToSpawn <= 0)
        {
            return;
        }

        SpawnNPCsAsync();
    }

    private async void SpawnNPCsAsync()
    {
        AssemblyStation station = AssemblyStationManager.Instance.GetCurrentStation();

        for (int i = 0; i < _numNPCsToSpawn; ++i)
        {
            await Task.Delay(250);

            if (i > _maxNPCsToSpawn)
            {
                Debug.LogError("SpawnNPCsAsync ran over the _maxNPCsToSpawn! Exiting spawn loop early");
                break;
            }

            SpawnNPC(station);
        }
    }

    private void SpawnNPC(AssemblyStation station)
    {
        if (station == null)
        {
            Debug.LogError("SpawnNPC returned early because station was null!");
            return;
        }

        station.SpawnNPC();
    }

    public void AddNPC()
    {
        _numNPCsToSpawn++;

        // Spawn new NPC immediately
        if (_numNPCsToSpawn < _maxNPCsToSpawn)
        {
            AssemblyStation station = AssemblyStationManager.Instance.GetCurrentStation();
            SpawnNPC(station);
        }
    }
    
    public void RemoveNPC()
    {
        if (_numNPCsToSpawn > 0)
        {
            _numNPCsToSpawn--;
            Debug.Log("Removing NPC to spawn. Now: " + _numNPCsToSpawn);
        }
    }
}
