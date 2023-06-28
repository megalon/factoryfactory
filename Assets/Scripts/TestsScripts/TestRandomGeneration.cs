using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandomGeneration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int numProducts = StringUtils.productPromptOptions.Count + 3;
        Debug.Log($"Testing GetListOfProducts using {numProducts}");
        int i = 0;
        foreach (string item in RandomizationManager.Instance.GetRandomListOfProducts(numProducts))
        {
            Debug.Log($"{i} {item}");
            ++i;
        }
    }
}