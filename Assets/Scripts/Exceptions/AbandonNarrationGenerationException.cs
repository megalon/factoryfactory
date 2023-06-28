using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonNarrationGenerationException : Exception
{
    public AbandonNarrationGenerationException()
    {
    }

    public AbandonNarrationGenerationException(string message)
        : base(message)
    {
    }

    public AbandonNarrationGenerationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
