using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModerationAPIFlaggedException : Exception
{
    public ModerationAPIFlaggedException()
    {
    }

    public ModerationAPIFlaggedException(string message)
        : base(message)
    {
    }

    public ModerationAPIFlaggedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}