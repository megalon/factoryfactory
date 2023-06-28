using System.Collections;
using System.Collections.Generic;
using Twitchmata.Models;
using UnityEngine;

public class VoiceChangeRedeem : UserSpecificRedeem
{
    private string _voice;
    public string Voice { get => _voice; }

    public VoiceChangeRedeem(ChannelPointRedemption redemption, string voice) : base(redemption)
    {
        _modificationType = ModificationTypes.AUDIO;
        _voice = voice;
    }

    protected override void OnActivate()
    {
        Debug.LogWarning("VoiceChangeRedeem changing voice to " + Voice);

        // We need to change the voice immediately so that it doesn't wait for the API check
        VoiceChangeManager.Instance.SetNextVoiceId(_voice, true);
    }

    protected override void OnDeactivate()
    {
        Debug.LogWarning("VoiceChangeRedeem deactivating");
        VoiceChangeManager.Instance.SetNextVoiceId(VoiceChangeManager.Instance.DefaultVoice);
    }
}
