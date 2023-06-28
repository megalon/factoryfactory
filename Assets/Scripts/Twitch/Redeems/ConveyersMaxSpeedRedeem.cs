using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class ConveyersMaxSpeedRedeem : TimedRedeem
{
#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 60;
#endif

    public ConveyersMaxSpeedRedeem(ChannelPointRedemption redemption) : base(redemption) { }

    protected override void OnActivate()
    {
        WackyModificationsManager.Instance.SetMaxConveyerbeltSpeed();
    }

    protected override void OnDeactivate()
    {
        WackyModificationsManager.Instance.SetRandomConveyerbeltSpeed();
    }
}
