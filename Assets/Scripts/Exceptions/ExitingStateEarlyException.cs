using System;

public class ExitingStateEarlyException : Exception
{
    public ExitingStateEarlyException()
    {
    }

    public ExitingStateEarlyException(string message)
        : base(message)
    {
    }

    public ExitingStateEarlyException(string message, Exception inner)
        : base(message, inner)
    {
    }
}