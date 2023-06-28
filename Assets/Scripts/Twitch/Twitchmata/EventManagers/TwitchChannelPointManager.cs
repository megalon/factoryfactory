using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Twitchmata;
using Twitchmata.Models;
using TwitchLib.Api.Core.Enums;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Linq;

public class TwitchChannelPointManager : ChannelPointManager
{
    public static TwitchChannelPointManager Instance;

    public ManagedReward LowGravityReward;
    public ManagedReward ConveyerBeltsMaxSpeed;
    public ManagedReward SmallBoxes;
    public ManagedReward PutNPCOnConveyerBelt;
    public ManagedReward VoiceChange;

    private Twitchmata.Models.User _testUser;
    public Dictionary<string, (string, float)> _voicesDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;

        // We need to map the pretty names to the actual uberduck names
        // The tuple contains the ID and volume

        // Ideally this would be in a text file, probably per-TTS system
        // but this is defined here to avoid any race conditions.
        // The redeem is initialized in InitializeFeatureManager, and needs access to this immediately
        // to write the description text
        _voicesDict = new Dictionary<string, (string, float)> {
            { "plankton", ("plankton", 0.85f) },
            { "computer", ("computer", 0.8f) },
            { "hank-hill", ("hank-hill", 1f) },
            { "glados", ("glados-p2", 0.332f) },
            { "dr-kleiner", ("isaac-kleiner", 0.5f) },
            { "cave-johnson", ("cave-johnson", 0.75f) },
            { "french-narrator", ("frenchy", 0.9f) }
        };
    }

    public override void InitializeFeatureManager()
    {
#if UNITY_EDITOR
        int defaultCooldown = 3;
#else
        int defaultCooldown = 60 * 3;
#endif

        LowGravityReward = new ManagedReward("Low Gravity", 200, Permissions.Everyone);
        LowGravityReward.Description = "Turn on low gravity for 1 minute of show time!";
        LowGravityReward.GlobalCooldownSeconds = defaultCooldown;
        RegisterReward(LowGravityReward, LowGravityRedeemMethod);

        ConveyerBeltsMaxSpeed = new ManagedReward("Conveyers MAX Speed", 200, Permissions.Everyone);
        ConveyerBeltsMaxSpeed.Description = "MAXIMUM speed conveyor belts for 1 minute of show time!";
        ConveyerBeltsMaxSpeed.GlobalCooldownSeconds = defaultCooldown;
        RegisterReward(ConveyerBeltsMaxSpeed, ConveyerBeltMaximumSpeedMethod);

        SmallBoxes = new ManagedReward("Small Boxes", 200, Permissions.Everyone);
        SmallBoxes.Description = "Smaller boxes for 1 minute of show time!";
        SmallBoxes.GlobalCooldownSeconds = defaultCooldown;
        RegisterReward(SmallBoxes, SmallBoxesMethod);

        PutNPCOnConveyerBelt = new ManagedReward("Put Me In Coach!", 500, Permissions.Everyone);
        PutNPCOnConveyerBelt.Description = "Throw a robot worker onto the assembly line for 1 minute of show time!";
        PutNPCOnConveyerBelt.GlobalCooldownSeconds = 5;
        RegisterReward(PutNPCOnConveyerBelt, PutNPCOnConveyerBeltMethod);

        List<string> voiceNames = _voicesDict.Keys.ToList();
        voiceNames.Sort();

        VoiceChange = new ManagedReward("Voice for Your Request", 50, Permissions.Everyone);
        VoiceChange.Description = $"EXAMPLE: 'glados laptops' | Options: {string.Join(", ", voiceNames)}";
        VoiceChange.GlobalCooldownSeconds = 2;
        VoiceChange.RequiresUserInput = true;
        VoiceChange.AutoFulfills = false;
        RegisterReward(VoiceChange, VoiceChangeRedeemMethod);

        _testUser = new Twitchmata.Models.User("12345", "TEST", "TESTNAME");

        DelayedStart();
    }

    private async void DelayedStart()
    {
        await Task.Delay(2000);

        // This was not working at start, but works if delayed
        // Likely a race condition
        DisableQueueBasedRewards();
    }

    private void LowGravityRedeemMethod(ChannelPointRedemption redemption, CustomRewardRedemptionStatus status)
    {
        TimedRedeemManager.Instance.AddRedeem(new LowGravityRedeem(redemption));
    }

    private void ConveyerBeltMaximumSpeedMethod(ChannelPointRedemption redemption, CustomRewardRedemptionStatus status)
    {
        TimedRedeemManager.Instance.AddRedeem(new ConveyersMaxSpeedRedeem(redemption));
    }

    private void SmallBoxesMethod(ChannelPointRedemption redemption, CustomRewardRedemptionStatus status)
    {
        TimedRedeemManager.Instance.AddRedeem(new SmallBoxRedeem(redemption));
    }

    private void PutNPCOnConveyerBeltMethod(ChannelPointRedemption redemption, CustomRewardRedemptionStatus status)
    {
        TimedRedeemManager.Instance.AddRedeem(new PutNPCOnConveyerRedeem(redemption));
    }

    private void VoiceChangeRedeemMethod(ChannelPointRedemption redemption, CustomRewardRedemptionStatus status)
    {
        // Cancel redeption if queue is closed
        if (!RequestsManager.Instance.IsQueueOpen)
        {
            SendReplyMessage(redemption.User, "Queue is closed! Refunding channel points.");
            CancelRedemption(redemption);
            return;
        }

        if (status == CustomRewardRedemptionStatus.CANCELED)
        {
            SendReplyMessage(redemption.User, "Invalid input! Refunding channel points.");
            CancelRedemption(redemption);
            return;
        }

        string input = redemption.UserInput;

        input = input.Trim();

        // If input is just the voice name with no request there will be no spaces
        if (!input.Contains(' '))
        {
            if (IsVoiceInDict(input))
            {
                SendReplyMessage(redemption.User, "Please include a request. Ex: 'computer laptops and car tires' Refunding channel points.");
            } else
            {
                SendReplyMessage(redemption.User, "Input invalid! Ex: 'computer laptops and car tires' Refunding channel points.");
            }
            CancelRedemption(redemption);
            return;
        }

        string voiceName = input.Substring(0, input.IndexOf(" ")).ToLower();

        if (!IsVoiceInDict(voiceName))
        {
            SendReplyMessage(redemption.User, "Voice not found! Refunding channel points.");
            CancelRedemption(redemption);
            return;
        }

        string voiceID = _voicesDict[voiceName].Item1;
        string request = input.Substring(voiceName.Length).Trim();

        VoiceRedeemRequestAsync(redemption, request, voiceID);
    }

    private async void VoiceRedeemRequestAsync(ChannelPointRedemption redemption, string request, string voiceID)
    {
        if (await TwitchChatCommandManager.Instance.TryModerateCommand("voice-redeem", request, redemption.User))
        {
            if (TwitchChatCommandManager.Instance.ProductRequest(request, redemption.User, voiceID))
            {
                return;
            }
        }

        // If the request was rejected by moderation, cancel the redeem
        CancelRedemption(redemption);
    }

    private bool IsVoiceInDict(string voice)
    {
        return _voicesDict.Where(x => x.Key == voice.ToLower()).Any();
    }

    private void SendReplyMessage(User user, string message)
    {
        string msg = $"{user.DisplayName} " + message;
        Debug.Log($"SendReplyMessage: " + msg);
        SendChatMessage(msg);
    }

    public void DisableQueueBasedRewards()
    {
        DisableReward(VoiceChange);
    }

    public void EnableQueueBasedRewards()
    {
        EnableReward(VoiceChange);
    }

    #region Test Scripts
#if UNITY_EDITOR
    public void TestLowGravity()
    {
        ChannelPointRedemption redemption = new ChannelPointRedemption();
        redemption.User = _testUser;
        redemption.RedemptionID = "0001";
        redemption.Reward = LowGravityReward;

        Debug.Log("TestLowGravity");

        LowGravityRedeemMethod(redemption, CustomRewardRedemptionStatus.UNFULFILLED);
    }

    public void TestMaxConveyerbeltSpeed()
    {
        ChannelPointRedemption redemption = new ChannelPointRedemption();
        redemption.User = _testUser;
        redemption.RedemptionID = "0002";
        redemption.Reward = ConveyerBeltsMaxSpeed;

        Debug.Log("TestMaxConveyerbeltSpeed");

        ConveyerBeltMaximumSpeedMethod(redemption, CustomRewardRedemptionStatus.UNFULFILLED);
    }

    public void TestSmallBoxes()
    {
        ChannelPointRedemption redemption = new ChannelPointRedemption();
        redemption.User = _testUser;
        redemption.RedemptionID = "0003";
        redemption.Reward = SmallBoxes;

        Debug.Log("TestSmallBoxes");

        SmallBoxesMethod(redemption, CustomRewardRedemptionStatus.UNFULFILLED);
    }

    public void TestPutNPCOnConveyerBelt()
    {
        ChannelPointRedemption redemption = new ChannelPointRedemption();
        redemption.User = _testUser;
        redemption.RedemptionID = "0004";
        redemption.Reward = PutNPCOnConveyerBelt;

        Debug.Log("TestPutNPCOnConveyerBelt");

        PutNPCOnConveyerBeltMethod(redemption, CustomRewardRedemptionStatus.UNFULFILLED);
    }

    public void TestVoiceChangeRedeem()
    {
        ChannelPointRedemption redemption = new ChannelPointRedemption();
        redemption.User = _testUser;
        redemption.RedemptionID = "0005";
        redemption.Reward = VoiceChange;

        Debug.Log("TestVoiceChangeRedeem");

        VoiceChangeRedeemMethod(redemption, CustomRewardRedemptionStatus.UNFULFILLED);
    }

    public void TestStartRedeemsForUser()
    {
        UserSpecificRedeemManager.Instance.StartRedeemsForUser(_testUser, Modification.ModificationTypes.SHOW);
    }

    public void TestStopRedeemsForUser()
    {
        UserSpecificRedeemManager.Instance.StopRedeemsForUser(_testUser, Modification.ModificationTypes.SHOW);
    }
#endif
    #endregion
}
