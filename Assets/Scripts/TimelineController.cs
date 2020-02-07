using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; 
//Communicates with SO_Hi tree nodes to match intent and orchestrate animations
//Interfaces with SO_Hi Tree_Container script
public class TimelineController : MonoBehaviour
{   [SerializeField]
    public PlayableDirector playableDirector;
    [SerializeField]
    public List<TimelineAsset> timelines;
    public SpeechManager speech;
    public AudioSource audioSource;


    //aggregated play function with audio and animations
    //PARAM is taid passed from SO node, it is the index location of the timeline asset, string response to TTS
    //TODO SALSA integration 
    public async void Play(int id, string response)
    {
        AudioClip clip = await speech.SpeakWithSDKPlugin(response);
        audioSource.clip = clip;
        audioSource.Play();
        PlayFromTimelines(id);
    }
    
    //Animations play method
    //PARAM is taid passed from SO node, it is the index location of the timeline asset 
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



