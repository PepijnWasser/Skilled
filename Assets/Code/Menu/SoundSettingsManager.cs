using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingsManager : SettingTab
{
    public AudioMixer audioMixer;
    public Sprite muteImage;
    public Sprite unMuteImage;

    List<SoundChannel> soundChannels = new List<SoundChannel>();
    SoundChannel masterChannel;

    public delegate void Changes(SettingTab tab);
    public static event Changes changeCreated;

    public void SwitchMute(SoundChannel channel)
    {
        if (channel.muted)
        {
            UnMute(channel);
            if (channel == masterChannel)
            {
                UnMuteNonMainChannels();
            }
            else if (GetNonMasterChannelsMuted() < soundChannels.Count - 1)
            {
                UnMute(masterChannel);
            }
        }
        else
        {
            Mute(channel);
            if (channel == masterChannel)
            {
                MuteNonMainChannels();
            }
            else if(GetNonMasterChannelsMuted() == soundChannels.Count - 1)
            {
                Mute(masterChannel);
            }
        }

        hasChanges = true;
        changeCreated?.Invoke(this);
    }

    public void SetVolume(SoundChannel channel, float newVolume)
    {
        float oldVolume;
        audioMixer.GetFloat(channel.volumeChannel, out oldVolume);

        UnMute(channel);

        if(oldVolume != newVolume)
        {
            audioMixer.SetFloat(channel.volumeChannel, newVolume);
            hasChanges = true;
            changeCreated?.Invoke(this);
        }
    }

    void Mute(SoundChannel channel)
    {
        audioMixer.SetFloat(channel.muteChannel, -80);
        channel.muteIcon.sprite = muteImage;
        channel.muted = true;
    }

    void UnMute(SoundChannel channel)
    {
        audioMixer.SetFloat(channel.muteChannel, 0);
        channel.muteIcon.sprite = unMuteImage;
        channel.muted = false;
    }

    public void SetMuteImage(SoundChannel channel)
    {
        if (channel.muted)
        {
            channel.muteIcon.sprite = muteImage;
        }
        else
        {
            channel.muteIcon.sprite = unMuteImage;
        }
    }

    void MuteNonMainChannels()
    {
        foreach (SoundChannel soundChannel in soundChannels)
        {
            if (soundChannel != masterChannel)
            {
                Mute(soundChannel);
            }
        }
    }

    void UnMuteNonMainChannels()
    {
        foreach (SoundChannel soundChannel in soundChannels)
        {
            if (soundChannel != masterChannel)
            {
                UnMute(soundChannel);
            }
        }
    }

    int GetNonMasterChannelsMuted()
    {
        int count = 0;
        foreach(SoundChannel soundChannel in soundChannels)
        {
            if(soundChannel.muteChannel != "Master Mute")
            {
                if (soundChannel.muted)
                {
                    count += 1;
                }
            }
        }
        return count;
    }

    public void AddSoundChannel(SoundChannel channel)
    {
        soundChannels.Add(channel);
        if(channel.volumeChannel == "Master Volume")
        {
            masterChannel = channel;
        }
    }

    public override void SaveSettings()
    {
        if (hasChanges)
        {
            Debug.Log("saving changes");
            foreach (SoundChannel channel in soundChannels)
            {
                float volume;
                audioMixer.GetFloat(channel.volumeChannel, out volume);
                PlayerPrefs.SetFloat(channel.volumeChannel, volume);

                float muted;
                audioMixer.GetFloat(channel.muteChannel, out muted);
                PlayerPrefs.SetFloat(channel.muteChannel, muted);
            }
        }
    }
}
