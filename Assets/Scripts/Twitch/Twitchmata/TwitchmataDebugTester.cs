using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitchmata;
using UnityEngine;

public class TwitchmataDebugTester : MonoBehaviour
{
    public bool testModeration;
    public bool testSubs;
    public bool testFollows;
    public bool testBits;
    public bool testRaids;
    public bool testChatCommands;

    private SubscriberManager _subscriberManager;
    private FollowManager _followManager;
    private BitsManager _bitsManager;
    private RaidManager _raidManager;

    private TwitchChatCommandManager _twitchChatCommandManager;

    private void Awake()
    {
        _subscriberManager = GetComponent<SubscriberManager>();
        _followManager = GetComponent<FollowManager>();
        _bitsManager = GetComponent<BitsManager>();
        _raidManager = GetComponent<RaidManager>();
        _twitchChatCommandManager = GetComponent<TwitchChatCommandManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        _ = TestDebug();
#endif
    }

    private void TestModeration(string text)
    {
        text = StringUtils.Sanitize(text);
        text = StringUtils.StrictSanitizeText(text, true);

        try
        {
            StringUtils.StrictModerationCheckThrowsError(text);
            Debug.Log("passed strict moderation! Text" + text);
        }catch 
        {
            Debug.LogWarning("Did not pass strict moderation! Text:" + text);
        }
    }

    private async Task TestDebug()
    {
        await Task.Delay(3000);

        Debug.Log("TestDebug...");

        if (testModeration)
        {
            TestModeration("fuck");
            TestModeration("fuck you");
            TestModeration("you fuck");
            TestModeration("f u c k");
            TestModeration("fuckfuck");
            TestModeration("hey .fuck. you");
            TestModeration("hey gofuckyouself bro");
            TestModeration("hi how are you");
            TestModeration("golden balls");
            TestModeration("eyeglasses");
            TestModeration("fuuck");
            TestModeration("fuckk");
            TestModeration("pornn");
            TestModeration("fumkhyou");
        }

        if (testFollows)
        {
            _followManager.Debug_NewFollow();
        }

        if (testSubs)
        {
            _subscriberManager.Debug_NewSubscription();
            _subscriberManager.Debug_NewGiftSubscription();
        }

        if (testBits)
        {
            _bitsManager.Debug_SendBits();
            _bitsManager.Debug_SendAnonymousBits();
        }

        if (testRaids)
        {
            _raidManager.Debug_IncomingRaid();
        }

        if (testChatCommands)
        {
            _twitchChatCommandManager.TestMessages();
        }
    }
}
