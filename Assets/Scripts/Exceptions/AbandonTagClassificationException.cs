using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonTagClassificationException : Exception
{
    public AbandonTagClassificationException()
    {
    }

    public AbandonTagClassificationException(string message)
        : base(message)
    {
    }

    public AbandonTagClassificationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
