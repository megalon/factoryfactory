using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimedRedeemManager : TimedModificationsManager
{
    public static TimedRedeemManager Instance;

    protected override void OnAwake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    public void AddRedeem(TimedRedeem redeem)
    {
        if (redeem == null)
        {
            Debug.LogError("Could not add null Redeem");
            return;
        }

        if (redeem.Redemption.User == null)
        {
            Debug.LogError("No user for Redemption!");
            return;
        }

        AddTimedModification(redeem);
    }

    protected override void OnUpdate() { }
    public override void OnTickClock(float deltaTime, StateMachine.StateIDs stateID) { }
}
