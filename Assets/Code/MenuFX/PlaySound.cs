using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    MenuSoundFXManager soundFXManager;

    private void Start()
    {
        soundFXManager = GameObject.FindObjectOfType<MenuSoundFXManager>();
    }

    public void PlayAudio()
    {
        soundFXManager.PlayAudio();
    }
}
