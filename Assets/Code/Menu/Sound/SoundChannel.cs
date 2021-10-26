using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundChannel : MonoBehaviour
{
    public SoundSettingsManager manager;

    public Slider slider;
    public Image muteIcon;
    public Text count;
    public string muteChannel;
    public string volumeChannel;

    public bool muted;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        float soundVolume = PlayerPrefs.GetFloat(volumeChannel);

        float muteVolume = PlayerPrefs.GetFloat(muteChannel);
        if (muteVolume == -80)
        {
            muted = true;
        }
        else
        {
            muted = false;
        }
        manager.SetInitialMuteImage(this);
        manager.SetVolumeNoAlert(this, soundVolume);
    }

    public void SetVolume(float newVolume)
    {
        manager.SetVolume(this, newVolume);
    }

    public void SwitchMute()
    {
        manager.SwitchMute(this);
    }
}
