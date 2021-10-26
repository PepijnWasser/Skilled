using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettingsManager : SettingsTab
{
    public AudioMixer audioMixer;
    public Sprite muteImage;
    public Sprite unMuteImage;


    public List<SoundChannel> soundChannels = new List<SoundChannel>();
    SoundChannel masterChannel;

    public delegate void Changes(SettingsTab tab);
    public static event Changes changeCreated;


    private void Start()
    {
        soundChannels = new List<SoundChannel>(GameObject.FindObjectsOfType<SoundChannel>());
        foreach(SoundChannel channel in soundChannels)
        {
            if (channel.volumeChannel == "Master Volume")
            {
                masterChannel = channel;
            }
        }
    }

    private void OnEnable()
    {
        foreach(SoundChannel channel in soundChannels)
        {
            channel.Reset();
        }
    }


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

            float newValue = 100 * ((80 + newVolume) / 80); 
            channel.count.text = ((int)newValue).ToString();

            hasChanges = true;
            changeCreated?.Invoke(this);
        }
    }

    public override void RestoreDefaults()
    {
        foreach(SoundChannel channel in soundChannels)
        {
            UnMute(channel);
            SetVolumeNoAlert(channel, 0);
        }
        changeCreated?.Invoke(this);
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

    public void SetInitialMuteImage(SoundChannel channel)
    {
        if (channel.muted)
        {
            channel.muteIcon.sprite = muteImage;
            audioMixer.SetFloat(channel.muteChannel, -80);
        }
        else
        {
            channel.muteIcon.sprite = unMuteImage;
            audioMixer.SetFloat(channel.muteChannel, 0);
        }
    }

    public void SetVolumeNoAlert(SoundChannel channel, float newVolume)
    {
        audioMixer.SetFloat(channel.volumeChannel, newVolume);
        channel.slider.SetValueWithoutNotify(newVolume);

        float newValue = 100 * ((80 + newVolume) / 80);
        channel.count.text = ((int)newValue).ToString();
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
            if(soundChannel != masterChannel)
            {
                if (soundChannel.muted)
                {
                    count += 1;
                }
            }
        }
        return count;
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
        else
        {
            Debug.Log("no changes detected");
        }
    }
}
