using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationPart
{
    public string text;
    public AudioClip audioClipUberduck; 

    public NarrationPart(string text)
    {
        this.text = text;
    }

    public void setUberduckAudioclip(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            Debug.LogError("Got empty audioclip in setAudioClip!");
            return;
        }
        Debug.Log("Setting AudioClip for:" + text);
        //Debug.Log("AudioClip length:" + audioClip.length);
        audioClipUberduck = audioClip;
    }
}
