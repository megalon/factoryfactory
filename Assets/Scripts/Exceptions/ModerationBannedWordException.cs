using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModerationBannedWordException : Exception
{
    public ModerationBannedWordException()
    {
    }

    public ModerationBannedWordException(string message)
        : base(message)
    {
    }

    public ModerationBannedWordException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
