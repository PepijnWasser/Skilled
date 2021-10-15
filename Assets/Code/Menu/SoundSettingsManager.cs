using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sprite muteImage;
    public Sprite unMuteImage;

    public Image masterMuteImage;
    public Image ambienceMuteImage;
    public Image musicMuteImage;


    public void SetMasterVolume(float newVolume)
    {
        audioMixer.SetFloat("Master Volume", newVolume);
        UnMute("Master Mute", masterMuteImage);
    }

    public void SetAmbienceVolume(float newVolume)
    {
        audioMixer.SetFloat("Ambience Volume", newVolume);
        UnMute("Ambience Mute", ambienceMuteImage);
    }

    public void SetMusicVolume(float newVolume)
    {
        audioMixer.SetFloat("Music Volume", newVolume);
        UnMute("Music Mute", musicMuteImage);
    }

    public void SwitchMasterMute()
    {
        SetMute("Master Mute", masterMuteImage);
    }

    public void SwitchAmbienceMute()
    {
        SetMute("Ambience Mute", ambienceMuteImage);
    }

    public void SwitchMusicMute()
    {
        SetMute("Music Mute", musicMuteImage);
    }

    void SetMute(string channelName, Image channelImage)
    {
        float muteVolume = 0;
        audioMixer.GetFloat(channelName, out muteVolume);
        if (muteVolume == 0)
        {
            audioMixer.SetFloat(channelName, -80);
            channelImage.sprite = muteImage;

            if (channelName == "Master Mute")
            {
                Mute("Music Mute", musicMuteImage);
                Mute("Ambience Mute", ambienceMuteImage);
            }
        }
        else
        {
            audioMixer.SetFloat(channelName, 0);
            channelImage.sprite = unMuteImage;

            if (channelName == "Master Mute")
            {
                UnMute("Music Mute", musicMuteImage);
                UnMute("Ambience Mute", ambienceMuteImage);
            }
            else
            {
                UnMute("Master Mute", masterMuteImage);
            }
        }
    }

    void Mute(string channelName, Image channelImage)
    {
        audioMixer.SetFloat(channelName, -80);
        channelImage.sprite = muteImage;
    }

    void UnMute(string channelName, Image channelImage)
    {
        audioMixer.SetFloat(channelName, 0);
        channelImage.sprite = unMuteImage;
    }

    void GetMuteAmount()
    {

    }
}
