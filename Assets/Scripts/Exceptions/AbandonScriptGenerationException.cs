using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonScriptGenerationException : Exception
{
    public AbandonScriptGenerationException()
    {
    }

    public AbandonScriptGenerationException(string message)
        : base(message)
    {
    }

    public AbandonScriptGenerationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
