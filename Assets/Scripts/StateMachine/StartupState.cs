using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartupState : IState
{
    public int MinimumTimeInStateSec => 0;
    public StateMachine.StateIDs ID => StateMachine.StateIDs.STARTUP;

    public async Task EnterState(StateMachine context)
    {
        MainScriptHandler.Instance.GenerateNewScript();

        UIManager.Instance.ClearUI();

        CameraManager.Instance.StateChanged(ID);
    }

    public StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.INTERMISSION;
    }

    public bool CheckTransition(StateMachine context, out StateMachine.StateIDs nextStateID)
    {
        nextStateID = ID;

        if (MainScriptHandler.Instance.IsNextScriptReady())
        {
            nextStateID = GetNextState();
            return true;
        }
        
        return false;
    }

    public void UpdateState(StateMachine context) { }
    public async Task ExitState(StateMachine context) { }
    void IState.ExitImmediately(StateMachine context) { }
}
