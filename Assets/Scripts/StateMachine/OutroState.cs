using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OutroState : ScriptPlayingState
{
    public override StateMachine.StateIDs ID => StateMachine.StateIDs.OUTRO;

    public override async Task EnterState(StateMachine context)
    {
        finishedPlayingAudio = false;

        _ = PlaySectionDialog(2);
    }

    public override void UpdateState(StateMachine context)
    {
        TimedRedeemManager.Instance.TickClock(Time.deltaTime, ID);
        TwitchSpecialEventManager.Instance.TickClock(Time.deltaTime, ID);
    }

    public override StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.END;
    }

    public override async Task ExitState(StateMachine context) { }
}
