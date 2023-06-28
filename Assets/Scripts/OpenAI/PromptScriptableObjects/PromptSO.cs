using UnityEngine;

[CreateAssetMenu(fileName = "Prompt", menuName = "ScriptableObjects/PromptSO", order = 1)]
public class PromptSO : ScriptableObject
{
    [TextArea(5, 50)]
    public string text;
    public string textToPrepend;
    public string modelName;
    public float temperature;
    public int maxLength;
    public string textFieldName;
    //public SOCompletionArgsV1 completionArgs;
}