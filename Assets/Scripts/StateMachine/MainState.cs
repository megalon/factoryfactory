using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainState : ScriptPlayingState
{
    public override int MinimumTimeInStateSec => 5;
    public override StateMachine.StateIDs ID => StateMachine.StateIDs.MAIN;

    public override async Task EnterState(StateMachine context)
    {
        finishedPlayingAudio = false;

        UIManager.Instance.SetScriptInfoInUI();

        CameraManager.Instance.MoveCameraToAssemblyStation(
            AssemblyStationManager.Instance.GetCurrentStation()
        );

        // Start redeems if this is a request
        if (MainScriptHandler.Instance.currentScript.request != null)
        {
            UserSpecificRedeemManager.Instance.StartRedeemsForUser(MainScriptHandler.Instance.currentScript.request.User, Modification.ModificationTypes.SHOW);
        }

        _ = PlaySectionDialog(1);
    }

    protected override async Task<bool> PlaySectionDialog(int sectionNum)
    {
        await TransitionFadeBlack.Instance.FadeFromBlackAsync();

        ScriptSection section = MainScriptHandler.Instance.currentScript.scriptSections[sectionNum];

        int partCounter = 0;
        foreach (NarrationPart narrationPart in section.narrationParts)
        {
            try
            {
                await MainScriptHandler.Instance.PlayNarrationPartAudioAsync(narrationPart);
            } catch(ExitingStateEarlyException)
            {
                Debug.Log("Caught ExitingStateEarlyException!");
                break;
            }

            ++partCounter;

            // Only move on to next section if it is not the last narration part
            // For the outro we hold on the last station
            if (partCounter < section.narrationParts.Count)
            {
                AssemblyStationManager.Instance.MoveToNextAssemblyStation();
            }
        }

        finishedPlayingAudio = true;
        return finishedPlayingAudio;
    }

    public override void UpdateState(StateMachine context)
    {
        TimedRedeemManager.Instance.TickClock(Time.deltaTime, ID);
        TwitchSpecialEventManager.Instance.TickClock(Time.deltaTime, ID);
    }

    public override async Task ExitState(StateMachine context)
    {
        UIManager.Instance.SetSubtitles("");
    }

    public override StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.OUTRO;
    }
}
