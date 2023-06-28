using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationManager : MonoBehaviour
{
    public static RandomizationManager Instance;

    private Queue<string> randomLocations;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public string GetRandomLocation()
    {
        return StringUtils.locations[UnityEngine.Random.Range(0, StringUtils.locations.Count)];
    }

    public string GetRandomProductPrompt()
    {
        return StringUtils.productPromptOptions[UnityEngine.Random.Range(0, StringUtils.productPromptOptions.Count)];
    }

    public List<string> GetRandomListOfProducts(int listSize)
    {
        int randOffset = Random.Range(0, StringUtils.productPromptOptions.Count);

        List<string> productList = new List<string>();

        for (int i = 0; i < listSize; ++i)
        {
            int productIndex = (randOffset + i) % StringUtils.productPromptOptions.Count;
            productList.Add(StringUtils.productPromptOptions[productIndex]);
        }

        return productList;
    }
}
