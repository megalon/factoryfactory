using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ScriptPlayingState : IState
{
    protected bool finishedPlayingAudio = false;
    public virtual int MinimumTimeInStateSec => 2;
    public abstract StateMachine.StateIDs ID { get; }
    private bool exitingImmediately = false;

    protected virtual async Task<bool> PlaySectionDialog(int sectionNum)
    {
        try
        {
            await MainScriptHandler.Instance.PlayScriptSectionAudioAsync(
                MainScriptHandler.Instance.currentScript.scriptSections[sectionNum]
            );
        } catch (ExitingStateEarlyException)
        {
            Debug.LogWarning("Exiting state early!");
        }
        finishedPlayingAudio = true;

        return true;
    }

    public bool CheckTransition(StateMachine context, out StateMachine.StateIDs nextStateID)
    {
        nextStateID = ID;

        if (finishedPlayingAudio)
        {
            if (exitingImmediately)
            {
                nextStateID = StateMachine.StateIDs.END;
                exitingImmediately = false;
            } else
            {
                nextStateID = GetNextState();
            }
            return true;
        }

        return false;
    }

    public virtual void ExitImmediately(StateMachine context)
    {
        exitingImmediately = true;
        MainScriptHandler.Instance.ExitStateImmediately();
    }

    public abstract Task EnterState(StateMachine context);
    public abstract Task ExitState(StateMachine context);
    public abstract StateMachine.StateIDs GetNextState();
    public abstract void UpdateState(StateMachine context);

}
