using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public abstract class UserSpecificRedeem : Modification, IRedeem
{
    private ChannelPointRedemption _redemption;
    public ChannelPointRedemption Redemption { get => _redemption; }

    public UserSpecificRedeem(ChannelPointRedemption redemption)
    {
        _redemption = redemption;
    }
}
