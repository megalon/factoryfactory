using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public abstract class SpecialEvent : TimedModification
{
    protected User user;

    public SpecialEvent(User user) : base()
    {
        this.user = user;
    }
}
