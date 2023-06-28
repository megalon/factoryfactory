using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IState
{
    public abstract int MinimumTimeInStateSec { get; }
    public abstract StateMachine.StateIDs ID { get; }
    public abstract bool CheckTransition(StateMachine context, out StateMachine.StateIDs nextStateID);
    public abstract Task EnterState(StateMachine context);

    public abstract void UpdateState(StateMachine context);

    public abstract Task ExitState(StateMachine context);

    public abstract void ExitImmediately(StateMachine context);

    // I think we only need this for the test button
    public abstract StateMachine.StateIDs GetNextState();
}
