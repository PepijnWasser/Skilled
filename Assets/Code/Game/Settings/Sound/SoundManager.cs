using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        InputManager.savedControls.Game.IncreaseVolume.performed += _ => IncreaseVolume();
        InputManager.savedControls.Game.DecreaseVolume.performed += _ => DecreaseVolume();
    }

    private void OnDestroy()
    {
        InputManager.savedControls.Game.IncreaseVolume.performed -= _ => IncreaseVolume();
        InputManager.savedControls.Game.DecreaseVolume.performed -= _ => DecreaseVolume();
    }

    void IncreaseVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("Master", out masterVolume);
        if (masterVolume < 0)
        {
            audioMixer.SetFloat("Master", masterVolume + 1);
        }
    }

    void DecreaseVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("Master", out masterVolume);
        if (masterVolume > -80)
        {
            audioMixer.SetFloat("Master", masterVolume - 1);
        }
    }
}
