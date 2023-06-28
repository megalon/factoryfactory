using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedModificationsManager : MonoBehaviour
{
    public bool tickClockAllTheTime = false;

    private List<TimedModification> _timedMods;
    private List<TimedModification> _timedModsToRemove;

    private void Awake()
    {
        OnAwake();

        _timedMods = new List<TimedModification>();
        _timedModsToRemove = new List<TimedModification>();

// Failsafe to make sure whatever this is set to is not active in live stream
#if UNITY_STANDALONE
        tickClockAllTheTime = false;
#endif
    }

    private void Update()
    {
        if (tickClockAllTheTime)
        {
            TickClock(Time.deltaTime, StateMachine.StateIDs.MAIN);
        }

        OnUpdate();
    }

    /// <summary>
    /// Reduce time on TimedModifications. This should only be called when the main show is visible.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void TickClock(float deltaTime, StateMachine.StateIDs stateID)
    {
        _timedModsToRemove.Clear();

        foreach (TimedModification timedMod in _timedMods)
        {
            if (stateID == StateMachine.StateIDs.INTRO && !timedMod.TickClockInIntro)
            {
                continue;
            }

            timedMod.TickClock(deltaTime);
            if (timedMod.IsExpired())
            {
                _timedModsToRemove.Add(timedMod);
            }
        }

        foreach (TimedModification timedMod in _timedModsToRemove)
        {
            _timedMods.Remove(timedMod);
        }

        OnTickClock(deltaTime, stateID);
    }

    protected void AddTimedModification(TimedModification timedMod)
    {
        if (timedMod == null)
        {
            Debug.LogError("Could not add null TimedModification");
            return;
        }

        timedMod.Activate();
        _timedMods.Add(timedMod);
    }

    protected abstract void OnAwake();
    protected abstract void OnUpdate();
    public abstract void OnTickClock(float deltaTime, StateMachine.StateIDs stateID);
}
