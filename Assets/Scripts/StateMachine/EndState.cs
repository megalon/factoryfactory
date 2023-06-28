using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndState : IState
{
    public int MinimumTimeInStateSec => 0;

    public StateMachine.StateIDs ID => StateMachine.StateIDs.END;

    public bool CheckTransition(StateMachine context, out StateMachine.StateIDs nextStateID)
    {
        // We do everything in the Enter state, so just exit immediately
        nextStateID = GetNextState();
        return true;
    }

    public async Task EnterState(StateMachine context)
    {
        await TransitionFadeBlack.Instance.FadeToBlackAsync();
        AssemblyStationManager.Instance.ClearAssemblyStations();

        if (MainScriptHandler.Instance.currentScript.request != null)
        {
            UserSpecificRedeemManager.Instance.StopRedeemsForUser(MainScriptHandler.Instance.currentScript.request.User, Modification.ModificationTypes.SHOW);
        }

        UIManager.Instance.ClearUI();
    }

    public StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.INTERMISSION;
    }

    public void ExitImmediately(StateMachine context) { }
    public async Task ExitState(StateMachine context) { }
    public void UpdateState(StateMachine context) { }
}
