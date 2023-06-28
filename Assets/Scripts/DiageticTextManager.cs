using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiageticTextManager : MonoBehaviour
{
    public static DiageticTextManager Instance;

    [SerializeField]
    private TextMeshPro _introText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void SetIntroText(string text)
    {
        _introText.text = text;
    }
}
