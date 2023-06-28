using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class AssemblyStationManager : ExitableMonobehaviour
{
    public static AssemblyStationManager Instance;

    [SerializeField]
    private int _stationSpacing = 20;

    [SerializeField]
    private GameObject baseGameObjectToSpawn;
    private GameObject modifiedObjectToSpawn;

    [SerializeField]
    private List<GameObject> _assemblyStationChoices;

    private List<AssemblyStation> _assemblyStationRandomChoices;

    private List<AssemblyStation> _assemblyStations;
    private Queue<AssemblyStation> _assemblyStationsQueue;
    private int _stationCounter;

    private Dictionary<string, List<AssemblyStation>> _stationsTagMap;
    [SerializeField]
    private PromptSO _assemblyClassificationPrompt;

    private float _conveyerbeltSpeed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _assemblyStationsQueue = new Queue<AssemblyStation>();

        ClearAssemblyStations();

        // Build the list of random stations to pick from
        _assemblyStationRandomChoices = new List<AssemblyStation>();
        foreach (GameObject obj in _assemblyStationChoices)
        {
            AssemblyStation station = obj.GetComponent<AssemblyStation>();
            if (station.GetIncludeInRandomSelection())
            {
                _assemblyStationRandomChoices.Add(station);
            }
        }

        GenerateTagMap();
    }

    private void GenerateTagMap()
    {
        _stationsTagMap = new Dictionary<string, List<AssemblyStation>>();
        for (int i = 0; i < _assemblyStationChoices.Count; ++i)
        {
            GameObject obj = _assemblyStationChoices[i];
            AssemblyStation station = obj.GetComponent<AssemblyStation>();
            foreach (string tag in station.GetTags())
            {
                List<AssemblyStation> value = null;
                if (_stationsTagMap.TryGetValue(tag, out value))
                {
                    // Add this item to this list if it exists
                    value.Add(station);
                } else
                {
                    // Create new list if it doesn't exist
                    List<AssemblyStation> list = new List<AssemblyStation>();
                    list.Add(station);
                    _stationsTagMap.Add(tag, list);
                }
            }
        }
        Debug.Log("Generated tag map with tags:");
        Debug.Log(GetTagMapTagsString());
    }

    public static string GetTagMapTagsString()
    {
        // Join all the keys together in a comma separated list
        return string.Join(", ", Instance._stationsTagMap.Select(pair => $"{pair.Key}"));
    }

    public void MoveToNextAssemblyStation()
    {
        // Get rid of station we were just at
        Destroy(_assemblyStationsQueue.Dequeue().gameObject);

        if (_assemblyStationsQueue.Count == 0 && _stationCounter >= _assemblyStations.Count)
        {
            return;
        }

        Debug.Log("Moving to next assembly station");

        try
        {
            MainSceneActions.OnNextAssemblyStation();
        } catch
        {
            // Error when nothing is subscribed to the action
        }

        // Move camera to next station
        CameraManager.Instance.MoveCameraToAssemblyStation(GetCurrentStation());

        if (_stationCounter < _assemblyStations.Count)
        {
            // Generate the station after the one we just moved to
            GenerateNextStation();
        }
    }

    public AssemblyStation GetCurrentStation()
    {
        if (_assemblyStationsQueue.Count > 0)
        {
            return _assemblyStationsQueue.Peek();
        }

        Debug.LogError("Could not get current AssemblyStation. _assemblyStations is empty!");

        return null;
    }

    public void ClearAssemblyStations()
    {
        _stationCounter = 0;

        if (_assemblyStationsQueue == null)
            return;

        while (_assemblyStationsQueue.Count > 0)
        {
            Destroy(_assemblyStationsQueue.Dequeue().gameObject);
        }

        _assemblyStationsQueue.Clear();
    }

    public void InitTestStations()
    {
        List<AssemblyStation> stations = new List<AssemblyStation>();
        foreach (GameObject obj in _assemblyStationChoices)
        {
            stations.Add(obj.GetComponent<AssemblyStation>());
        }

        InitStations(stations);
    }

    public void InitStations(List<AssemblyStation> assemblyStationInput)
    {
        Debug.Log("InitStations...");

        _assemblyStations = assemblyStationInput;

        if (_assemblyStations == null)
        {
            Debug.LogError("_assemblyStations is null! This should not happen.");
        }

        if (modifiedObjectToSpawn != null)
        {
            Destroy(modifiedObjectToSpawn);
        }

        Debug.Log("InitStations: Handling the transformations...");

        modifiedObjectToSpawn = InstantiateObjectWithTransformations(baseGameObjectToSpawn);
        modifiedObjectToSpawn.SetActive(false);

        if (!WackyModificationsManager.Instance.IsMaxConveyerSpeed)
        {
            WackyModificationsManager.Instance.SetRandomConveyerbeltSpeed();
        }

        // Generate first station
        GenerateNextStation();

        if (_assemblyStations.Count > 1)
        {
            // Generate 2nd station
            GenerateNextStation();
        }
        else
        {
            Debug.LogWarning("Only needed to generate one AssemblyStation!");
        }
    }

    /// <summary>
    /// Instantiate a GameObject with all of the transformations that will occur in this assembly line
    /// </summary>
    /// <param name="baseObj"></param>
    /// <returns></returns>
    public GameObject InstantiateObjectWithTransformations(GameObject baseObj)
    {
        GameObject newObj = Instantiate(baseObj);

        for (int i = 0; i < _assemblyStations.Count; ++i)
        {
            AddTransformationToObject(newObj, i);
        }

        return newObj;
    }

    /// <summary>
    /// Add the Transformation to the GameObject from the input AssemblyStation index
    /// </summary>
    /// <param name="obj">GameObject to add Transformation to</param>
    /// <param name="assemblyStationIndex">Index of the assembly station</param>
    public void AddTransformationToObject(GameObject obj, int assemblyStationIndex)
    {
        AssemblyStation station = _assemblyStations[assemblyStationIndex];

        if (station == null)
        {
            Debug.LogError("InitStations station is null!");
            return;
        }

        string transformationClassName = station.GetTransformationClassName();

        if (string.IsNullOrEmpty(transformationClassName))
        {
            return;
        }

        Transformation trans = null;

        switch (transformationClassName)
        {
            case nameof(TransformationFlatten):
                trans = obj.AddComponent<TransformationFlatten>();
                break;
            case nameof(TransformationBurn):
                trans = obj.AddComponent<TransformationBurn>();
                break;
            case nameof(TransformationChop):
                trans = obj.AddComponent<TransformationChop>();
                break;
            case nameof(TransformationMelt):
                trans = obj.AddComponent<TransformationMelt>();
                break;
            case nameof(TransformationPackage):
                trans = obj.AddComponent<TransformationPackage>();
                break;
        }

        if (trans != null)
        {
            trans.index = assemblyStationIndex;
            trans.enabled = false;
        }
    } 

    public async Task<List<AssemblyStation>> GenerateAssemblyStationsList(ScriptSection scriptSection)
    {
        List<AssemblyStation> stationList = new List<AssemblyStation>();

        int loadingBarBudget = 33 / scriptSection.narrationParts.Count;

        for (int i = 0; i < scriptSection.narrationParts.Count; ++i)
        {
            if (stopGeneratingImmediately)
            {
                stopGeneratingImmediately = false;
                throw new AbandonTagClassificationException();
            }

            NarrationPart part = scriptSection.narrationParts[i];

            // Get tag from OpenAI
            string result = await GetTagFromOpenAI(part);

            //string result = GetTagFromDumbRegexCheck(part);

            List<AssemblyStation> list = null;

            // Check if the result is actually in the tags
            if (!result.Equals("") && _stationsTagMap.TryGetValue(result, out list))
            {
                Debug.Log($"Found result {result} in tags!");
                // Get a random item from this list
                stationList.Add(list[Random.Range(0, list.Count)]);
            }
            else
            {
                Debug.LogWarning($"Result {result} was not in the tags dict!");
                if (_assemblyStationRandomChoices.Count > 0)
                {
                    stationList.Add(_assemblyStationRandomChoices[Random.Range(0, _assemblyStationRandomChoices.Count)]);
                } else
                {
                    // Backup, incase we have no random choices while testing
                    stationList.Add(_assemblyStationChoices[Random.Range(0, _assemblyStationChoices.Count)].GetComponent<AssemblyStation>());
                }
            }

            LoadingBarManager.Instance.AppendLoadingBarPercent(loadingBarBudget);
        }

        return stationList;
    }

    private string GetTagFromDumbRegexCheck(NarrationPart part)
    {
        List<string> foundTags = new List<string>();
        foreach (string tag in _stationsTagMap.Keys)
        {
            if (Regex.IsMatch(part.text, $"\b{tag}(ing|er|ed)?\b"))
            {
                foundTags.Add(tag);
            }
        }

        if (foundTags.Count > 0)
        {
            return foundTags[Random.Range(0, foundTags.Count)];
        }

        return "";
    }

    private async Task<string> GetTagFromOpenAI(NarrationPart part)
    {
        // Add the tags to the prompt
        string promptText = StringUtils.ReplaceTextInString("tags", GetTagMapTagsString(), _assemblyClassificationPrompt.text);

        // Add the narration part at the end
        promptText = promptText + part.text;

        string result = "";

        try
        {
            result = await OpenAICompleter.Instance.CreateCompletionAsync(promptText, _assemblyClassificationPrompt);
        }
        catch
        {
            Debug.LogError("Failed to generate classification for narration part. Using random instead");
            result = "";
        }

        // Really shoud just use a stop seq for this
        result = Regex.Replace(result, @"\.", "");
        result = Regex.Replace(result, "\"", "");
        result = result.ToLower();

        return result;
    }

    private void GenerateNextStation()
    {
        Debug.Log($"GenerateNextStation...");

        AssemblyStation assemblyStation = null;
        Transform spawnTransform = transform;

        if (_stationCounter > 0)
        {
            spawnTransform = GetCurrentStation().GetNextStationSpawnPosition();
        }

        Debug.Log($"Instantiating...");

        if (_assemblyStations == null || _assemblyStations.Count <= 0)
        {
            Debug.LogError("GenerateNextStation() _assemblyStations is empty or null!");
            return;
        }

        GameObject go = Instantiate(
            _assemblyStations[_stationCounter].gameObject,
            spawnTransform.position + (Vector3.forward * _stationSpacing),
            spawnTransform.rotation);

        assemblyStation = go.GetComponent<AssemblyStation>();
        assemblyStation.Initialize(_stationCounter, modifiedObjectToSpawn);
        assemblyStation.SetConveyerbeltSpeed(_conveyerbeltSpeed);

        _assemblyStationsQueue.Enqueue(assemblyStation);

        ++_stationCounter;

        Debug.Log($"Finished generating AssemblyStation.");
    }

    /// <summary>
    /// Set the conveyer belt speed for all conveyer belts
    /// </summary>
    /// <param name="speed">1.3f is default</param>
    public void SetConveyerBeltSpeed(float speed)
    {
        _conveyerbeltSpeed = speed;

        foreach (AssemblyStation station in _assemblyStationsQueue)
        {
            station.SetConveyerbeltSpeed(_conveyerbeltSpeed);
        }
    }
}
