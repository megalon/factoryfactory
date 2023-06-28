using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class LowGravityRedeem : TimedRedeem
{

#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 60;
#endif

    public LowGravityRedeem(ChannelPointRedemption redemption) : base (redemption) { }

    protected override void OnActivate()
    {
        Debug.Log("Activating LowGravityRedeem!");
        WackyModificationsManager.Instance.SetGravity(new Vector3(0, -2, 0));
    }

    protected override void OnDeactivate()
    {
        Debug.Log("Deactivating LowGravityRedeem!");
        WackyModificationsManager.Instance.ResetGravityToDefault();
    }
}
