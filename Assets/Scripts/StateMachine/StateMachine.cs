using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static StateMachine Instance;
    public static Action NextStateAction;
    public IState currentState { get; private set; }
    public OpenAIScriptBuilderV2 scriptBuilder { get; private set; }

    private bool _minimumTimeInStateElapsed = false;
    private bool _waitingForAsync = false;

    private StartupState _startupState;
    private IntermissionState _intermissionState;
    private IntroState _introState;
    private MainState _mainState;
    private OutroState _outroState;
    private EndState _endState;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        _startupState = new StartupState();
        _intermissionState = new IntermissionState();
        _introState = new IntroState();
        _mainState = new MainState();
        _outroState = new OutroState();
        _endState = new EndState();

        NextStateAction += NextState;

        scriptBuilder = GetComponent<OpenAIScriptBuilderV2>();

        _ = ChangeState(StateIDs.STARTUP);
    }

    public void EndShowImmediately()
    {
        currentState.ExitImmediately(this);
    }

    void Update()
    {
        if (_waitingForAsync) return;

        UpdateAsync();
    }

    private async void UpdateAsync()
    {
        _waitingForAsync = true;
        if (_minimumTimeInStateElapsed && currentState.CheckTransition(this, out StateIDs nextState))
        {
            await ChangeState(nextState);
        }

        currentState.UpdateState(this);
        _waitingForAsync = false;
    }

    private async Task ChangeState(StateIDs newStateID)
    {
        if (currentState != null)
        {
            Debug.Log($"Changing state from:{currentState.GetType().Name} to {newStateID.GetType().Name}");
            await currentState.ExitState(this);
        } else
        {
            Debug.Log($"Current state is null! Setting state to:{newStateID.GetType().Name}");
        }

        currentState = GetStateFromID(newStateID);

        MusicManager.Instance.PlayMusic(currentState.ID);

        await currentState.EnterState(this);

        WaitMinimumTimeInCurrentStateAsync();
    }

    private async void WaitMinimumTimeInCurrentStateAsync()
    {
        _minimumTimeInStateElapsed = false;
        await Task.Delay(currentState.MinimumTimeInStateSec * 1000);
        _minimumTimeInStateElapsed = true;
    }

    // This is only used for the test button in the test scene
    private void NextState()
    {
        _ = ChangeState(currentState.GetNextState());
    }

    private IState GetStateFromID(StateIDs stateID)
    {
        switch (stateID)
        {
            case StateIDs.STARTUP:
                return _startupState;
            case StateIDs.INTERMISSION:
                return _intermissionState;
            case StateIDs.INTRO:
                return _introState;
            case StateIDs.MAIN:
                return _mainState;
            case StateIDs.OUTRO:
                return _outroState;
            case StateIDs.END:
                return _endState;
        }

        Debug.LogError($"Could not get state for ID: {stateID}");

        return null;
    }

    public enum StateIDs
    {
        STARTUP,
        INTERMISSION,
        INTRO,
        MAIN,
        OUTRO,
        END
    }
}
