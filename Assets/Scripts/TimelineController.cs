using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; 
//[CreateAssetMenu]
public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public List<TimelineAsset> timelines;
    public SpeechManager speech;
    public AudioSource audioSource;

    public void Play(int id)
    {
    }

    public async void Play(int id, string response)
    {
        AudioClip clip = await speech.SpeakWithSDKPlugin(response);
        audioSource.clip = clip;
        audioSource.Play();
        PlayFromTimelines(id);
    }

    public void PlayFromTimelines(int index)
    {
        TimelineAsset selectedAsset;
        if (timelines.Count <= index)
        {
            selectedAsset = timelines[timelines.Count - 1];
        }
        else
        {
            selectedAsset = timelines[index];
            playableDirector.Play(selectedAsset);
        }
    }
}



