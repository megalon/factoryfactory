using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssemblyStation : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _textMesh;
    [SerializeField]
    private Transform _cameraPosition;
    [SerializeField]
    private Transform _nextStationSpawnPosition;
    [SerializeField]
    private string _transformationClassName;
    [SerializeField]
    private bool _includeInRandomSelection = false;

    private WorkPartSpawner[] _spawners;
    private AnimationTimeScaler[] _animationTimeScalers;
    private NPCOnConveyerSpawner[] _npcOnConveyerSpawners;
    private int _indexInAssemblyLine;

    private void Awake()
    {
        _spawners = GetComponentsInChildren<WorkPartSpawner>();
        _animationTimeScalers = GetComponentsInChildren<AnimationTimeScaler>();
        _npcOnConveyerSpawners = GetComponentsInChildren<NPCOnConveyerSpawner>();
    }

    public void Initialize(int num, GameObject gameobjectToSpawn)
    {
        _indexInAssemblyLine = num;
        _textMesh.text = $"#{_indexInAssemblyLine + 1}";

        for (int i = 0; i < _spawners.Length; ++i)
        {
            _spawners[i].SetObjectToSpawn(gameobjectToSpawn);
            _spawners[i].SetTransformationIndex(_indexInAssemblyLine);
        }
    }

    public Transform GetCameraPosition()
    {
        return _cameraPosition;
    }

    public Transform GetNextStationSpawnPosition()
    {
        return _nextStationSpawnPosition;
    }

    public string GetTransformationClassName()
    {
        return _transformationClassName;
    }

    public bool GetIncludeInRandomSelection()
    {
        return _includeInRandomSelection;
    }

    public int GetIndexInAssemblyLine()
    {
        return _indexInAssemblyLine;
    }

    public List<string> GetTags()
    {
        TagsList tagsList = GetComponent<TagsList>();

        if (tagsList)
        {
            return tagsList.GetTagsList();
        }

        Debug.Log($"Could not find TagsList component on {gameObject.name}!");
        return null;
    }

    public void SetConveyerbeltSpeed(float conveyerbeltSpeed)
    {
        Conveyerbelt[] conveyerbelts = GetComponentsInChildren<Conveyerbelt>();

        if (conveyerbelts == null)
        {
            Debug.Log("Could not RandomizeConveyerbeltSpeed because there are no conveyerbelts on this assembly station!");
            return;
        }

        for (int i = 0; i < conveyerbelts.Length; ++i)
        {
            conveyerbelts[i].SetSpeed(conveyerbeltSpeed);
        }

        for (int i = 0; i < _animationTimeScalers.Length; ++i)
        {
            _animationTimeScalers[i].SetTimeScale(conveyerbeltSpeed);
        }
    }

    public void SpawnNPC()
    {
        if (_npcOnConveyerSpawners.Length <= 0)
        {
            Debug.LogError("_npcOnConveyerSpawners array is empty! Could not spawn NPC on conveyer!");
            return;
        }

        _npcOnConveyerSpawners[0].SpawnNPC();
    }
}
