using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchSpecialEventManager : TimedModificationsManager
{
    public static TwitchSpecialEventManager Instance;

    protected override void OnAwake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public override void OnTickClock(float deltaTime, StateMachine.StateIDs stateID) { }

    protected override void OnUpdate() { }

    public void AddSpecialEvent(TimedModification timedModification)
    {
        AddTimedModification(timedModification);
    }
}
