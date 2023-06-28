using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntroState : ScriptPlayingState
{
    public override StateMachine.StateIDs ID => StateMachine.StateIDs.INTRO;

    public override async Task EnterState(StateMachine context)
    {
        finishedPlayingAudio = false;

        MainScriptHandler.Instance.NextScript();

        // If this script we are about to run is the latest request, remove the request from the queue
        TwitchRequest req;
        if (RequestsManager.Instance.ProductRequestList.TryPeek(out req))
        {
            if (req.RequestText == MainScriptHandler.Instance.currentScript.productNameScriptSection.text)
            {
                RequestsManager.Instance.ProductRequestList.RemoveAt(0);
            }
        }

        MainScriptHandler.Instance.GenerateNewScript();
        
        DiageticTextManager.Instance.SetIntroText(MainScriptHandler.Instance.currentScript.productNameScriptSection.text);

        CameraManager.Instance.StateChanged(ID);

        AssemblyStationManager.Instance.InitStations(
            MainScriptHandler.Instance.currentScript.assemblyStationsList
        ) ;

        VoiceChangeManager.Instance.SetVolumeForCurrentVoice();

        await TransitionFadeBlack.Instance.FadeFromBlackAsync();

        _ = PlaySectionDialog(0);
    }

    public override void UpdateState(StateMachine context)
    {
        TimedRedeemManager.Instance.TickClock(Time.deltaTime, ID);
        TwitchSpecialEventManager.Instance.TickClock(Time.deltaTime, ID);
    }

    public override async Task ExitState(StateMachine context)
    {
        await TransitionFadeBlack.Instance.FadeToBlackAsync();
        UIManager.Instance.SetSubtitles("");
    }

    public override StateMachine.StateIDs GetNextState()
    {
        return StateMachine.StateIDs.MAIN;
    }
}
