using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitchmata;
using Twitchmata.Models;
using UnityEngine;

public class TwitchFollowManager : FollowManager
{
    public override void UserFollowed(User follower)
    {
        base.UserFollowed(follower);
    }
}
