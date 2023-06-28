using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScriptSection
{
    public PromptSO promptSO;
    public List<NarrationPart> narrationParts;
    public string text { get; private set; }

    public ScriptSection(PromptSO promptSO)
    {
        this.promptSO = promptSO;

        // Narration parts are added after the script part is generated
        narrationParts = new List<NarrationPart>();
    }

    public void SetResultText(string textIn)
    {
        text = textIn;
    }
}
