using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public Animator musicAnimator;

    int intensity = 0;

    public List<AudioClip> calmClips;
    public List<AudioClip> mediumClips;
    public List<AudioClip> fastClips;

    public AudioSource musicPlayer;

    private void Awake()
    {
        if (ServerConnectionData.isOwner)
        {
            StationHealth.stationHalfWay += SetNextIntensity;
            StationHealth.stationThreeQuarterWay += SetNextIntensity;

            AudioClip newClip = Extensions.RandomListItem(calmClips);
            musicPlayer.clip = newClip;
            musicPlayer.Play();

            musicAnimator.SetTrigger("FadeIn");
        }
    }

    private void OnDestroy()
    {
        if (ServerConnectionData.isOwner)
        {
            StationHealth.stationHalfWay -= SetNextIntensity;
            StationHealth.stationThreeQuarterWay -= SetNextIntensity;
        }
    }

    void SetNextIntensity()
    {
        StartCoroutine("NextIntensity");
    }

    IEnumerator NextIntensity()
    {
        musicAnimator.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1);


        if (intensity < 2)
        {
            intensity += 1;
        }
        else
        {
            intensity = 0;
        }

        AudioClip newClip;
        if (intensity == 0)
        {
            newClip = Extensions.RandomListItem(calmClips);
        }
        else if (intensity == 1)
        {
            newClip = Extensions.RandomListItem(mediumClips);
        }
        else
        {
            newClip = Extensions.RandomListItem(fastClips);
        }
        musicPlayer.clip = newClip;
        musicPlayer.Play();

        musicAnimator.SetTrigger("FadeIn");


        yield return null;
    }
}
