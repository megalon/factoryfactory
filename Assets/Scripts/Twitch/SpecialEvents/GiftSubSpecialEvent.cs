using System;
using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class GiftSubSpecialEvent : SpecialEvent
{

#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 5;
#endif

    public override bool TickClockInIntro => true;

    public GiftSubSpecialEvent(User user) : base(user) { }

    protected override void OnActivate()
    {
        Debug.Log("OnActivate GiftSubSpecialEvent");
        ConfettiManager.Instance.EnableConfetti();
    }

    protected override void OnDeactivate()
    {
        Debug.Log("OnDeactivate GiftSubSpecialEvent");
        ConfettiManager.Instance.DisableConfetti();
    }
}
