using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSoundFXManager : MonoBehaviour
{
    public AudioSource audioSource;

    private static MenuSoundFXManager _instance;

    public static MenuSoundFXManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }
}
