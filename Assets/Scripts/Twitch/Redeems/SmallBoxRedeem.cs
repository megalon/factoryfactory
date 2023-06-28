using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class SmallBoxRedeem : TimedRedeem
{
#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 60;
#endif

    public SmallBoxRedeem(ChannelPointRedemption redemption) : base(redemption) { }

    protected override void OnActivate()
    {
        WackyModificationsManager.Instance.SetWorkPartScale(new Vector3(0.33f, 0.33f, 0.33f));
    }

    protected override void OnDeactivate()
    {
        WackyModificationsManager.Instance.SetWorkPartScale(Vector3.one);
    }
}
