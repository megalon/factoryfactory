using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitchmata;
using Twitchmata.Models;
using System.Threading.Tasks;

public class TwitchSubsManager : SubscriberManager
{
    public override void UserSubscribed(User subscriber)
    {
        base.UserSubscribed(subscriber);

        if (subscriber.Subscription.IsGift)
        {
            TwitchSpecialEventManager.Instance.AddSpecialEvent(new GiftSubSpecialEvent(subscriber));
        } else
        {
            TwitchSpecialEventManager.Instance.AddSpecialEvent(new SubSpecialEvent(subscriber));
        }
    }
}
