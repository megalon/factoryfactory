using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedModification : Modification
{
    public float timeLeft;

    public abstract int TimeLimitSec { get; }
    public virtual bool TickClockInIntro { get => false; }

    public override void Activate()
    {
        timeLeft = TimeLimitSec;
        base.Activate();
    }

    public void TickClock(float deltaTime)
    {
        timeLeft -= deltaTime;

        if (IsExpired())
        {
            Deactivate();
        }
    }

    public bool IsExpired()
    {
        return timeLeft <= 0;
    }
}
