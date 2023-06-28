using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchRequest
{
    public string RequestText { get; protected set; }
    public Twitchmata.Models.User User { get; protected set; }
    public string VoiceID { get; protected set; }

    public TwitchRequest(Twitchmata.Models.User user, string text, string voiceID)
    {
        RequestText = text;
        User = user;
        VoiceID = voiceID;
    }
}
