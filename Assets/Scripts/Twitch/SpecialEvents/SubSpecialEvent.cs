using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class SubSpecialEvent : SpecialEvent
{
#if UNITY_EDITOR
    public override int TimeLimitSec => 3;
#else
    public override int TimeLimitSec => 5;
#endif
    public override bool TickClockInIntro => true;

    public SubSpecialEvent(User user) : base(user) { }

    protected override void OnActivate()
    {
        Debug.Log("OnActivate SubSpecialEvent");
        ConfettiManager.Instance.EnableConfetti();
    }

    protected override void OnDeactivate()
    {
        Debug.Log("OnDeactivate SubSpecialEvent");
        ConfettiManager.Instance.DisableConfetti();
    }
}
