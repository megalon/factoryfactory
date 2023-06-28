using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitchmata.Models;
using UnityEngine;

public class PutNPCOnConveyerRedeem : TimedRedeem
{
#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 60;
#endif

    public PutNPCOnConveyerRedeem(ChannelPointRedemption redemption) : base(redemption) { }

    protected override void OnActivate()
    {
        NPCRedeemsManager.Instance.AddNPC();
    }

    protected override void OnDeactivate()
    {
        NPCRedeemsManager.Instance.RemoveNPC();
    }
}
