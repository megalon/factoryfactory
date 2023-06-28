using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyPizza;

public class TestPolyPizza : MonoBehaviour
{
    public async void GetModelAsync(string input)
    {
        Debug.Log("Downloading model...");
        Model model = await APIManager.instance.GetExactModel(input);
        Debug.Log("Making gameobject...");
        GameObject obj = await APIManager.instance.MakeModel(model);
    }
}
