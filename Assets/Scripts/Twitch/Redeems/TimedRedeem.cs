using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public abstract class TimedRedeem : TimedModification, IRedeem
{
    private ChannelPointRedemption _redemption;
    public ChannelPointRedemption Redemption { get => _redemption; }

    public TimedRedeem(ChannelPointRedemption redemption)
    {
        _redemption = redemption;
    }

    public override void Deactivate()
    {
        base.Deactivate();

        // Need to fulfill redeem so we don't refund it, however...
        // This always throws an error, and we're not even using this right now, so just ignore
        //TwitchChannelPointManager.Instance.FulfillRedemption(Redemption);
    }
}
