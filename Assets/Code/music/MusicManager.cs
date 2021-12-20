using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private Animator musicAnimator;

    [SerializeField]
    private List<AudioClip> calmClips;
    [SerializeField]
    private List<AudioClip> mediumClips;
    [SerializeField]
    private List<AudioClip> fastClips;

    [SerializeField]
    private AudioSource musicPlayer;

    private int intensity = 0;

    //if we are the owner of the room, we are the ones that play audio
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

    //unsubscribe from events when destroyed
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
        StartCoroutine(NextIntensity());
    }

    //the music intensity has 3 values which it loops through
    //the old music fades out, and the new music fades in
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
