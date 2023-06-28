using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntermissionState : IState
{
    public StateMachine.StateIDs ID => StateMachine.StateIDs.INTERMISSION;

    public int MinimumTimeInStateSec => 5;

    private bool _shouldShowLoadingBar = false;
    public async Task EnterState(StateMachine context)
    {
        VideoManager.Instance.PlayVideo(VideoManager.Videos.INTERMISSION);
        CameraManager.Instance.StateChanged(ID);
        await TransitionFadeBlack.Instance.FadeFromBlackAsync();
        WaitThenShowLoadingBar();
    }

    private async void WaitThenShowLoadingBar()
    {
        _shouldShowLoadingBar = true;
        await Task.Delay((MinimumTimeInStateSec + 1) * 1000);
        if (_shouldShowLoadingBar)
        {
            LoadingBarManager.Instance.ShowLoadingBar();
        }
    }

    public void UpdateState(StateMachine context)
    {

    }

    public async Task ExitState(StateMachine context)
    {
        _shouldShowLoadingBar = false;
        LoadingBarManager.Instance.SetLoadingBarPercent(100);
        await TransitionFadeBlack.Instance.FadeToBlackAsync();
        LoadingBarManager.Instance.HideLoadingBar();
        VideoManager.Instance.StopVideo();
    }

    public StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.INTRO;
    }

    public bool CheckTransition(StateMachine context, out StateMachine.StateIDs nextStateID)
    {
        nextStateID = ID;

        if (MainScriptHandler.Instance.IsNextScriptReady())
        {
            nextStateID = GetNextState();
            return true;
        } else
        {
            //Debug.LogWarning("Next script is not ready! Waiting...");
        }

        return false;
    }

    void IState.ExitImmediately(StateMachine context)
    {
        throw new System.NotImplementedException();
    }
}
