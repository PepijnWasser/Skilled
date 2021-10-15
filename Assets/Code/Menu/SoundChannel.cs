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
    public string muteChannel;
    public string volumeChannel;

    public bool muted;

    private void Start()
    {
        manager.AddSoundChannel(this);
        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(volumeChannel));

        float muteVolume = PlayerPrefs.GetFloat(muteChannel);
        if(muteVolume == -80)
        {
            muted = true;
        }
        else
        {
            muted = false;
        }
        manager.SetMuteImage(this);
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
