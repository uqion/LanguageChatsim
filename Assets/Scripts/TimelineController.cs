using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; 
//Communicates with SO_Hi tree nodes to match intent and orchestrate animations
//Interfaces with SO_Hi Tree_Container script
public class TimelineController : MonoBehaviour
{
    [SerializeField]
    public PlayableDirector playableDirector;
    [SerializeField]
    public List<TimelineAsset> timelines;
    [SerializeField]
    public List<TimelineAsset> WBtimelines; 
    public SpeechManager speech;
    public AudioSource audioSource;

    private Queue<BasicNode> queuedTimelines;
    private bool isPlaying = false;

    private void Awake()
    {
        queuedTimelines = new Queue<BasicNode>();
    }


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

    public async void Play(string response)
    {
        AudioClip clip = await speech.SpeakWithSDKPlugin(response);
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void Play(int id)
    {
        PlayFromTimelines(id);
    }


    //Animations play method
    //PARAM is taid passed from SO node, it is the index location of the timeline asset 
    //sets off a single timeline to play asynchronously
    private TimelineAsset PlayFromTimelines(int index)
    {
        TimelineAsset selectedAsset;
        if (timelines.Count <= index)
        {
            Debug.Log("No timeline for that index");
            selectedAsset = timelines[timelines.Count - 1];
        }
        else
        {
            selectedAsset = timelines[index];
        }
        playableDirector.Play(selectedAsset);
        return selectedAsset;
    }

    //sets off multiple timelines in sequence to play asynchronously
    //create 
    public void PlayFromTimelines(params BasicNode[] queue)
    {
        if (isPlaying)
        {
            Debug.Log("tried to play timelines while there are others playing");
            return;
        }
        foreach (BasicNode n in queue)
        {
            queuedTimelines.Enqueue(n);
        }
        StartCoroutine(playQueue());
    }

    private IEnumerator playQueue()
    {
        isPlaying = true;
        while (queuedTimelines.Count > 0)
        {
            BasicNode cur = queuedTimelines.Dequeue();
            Play(cur.getResponse());
            TimelineAsset currentTimeline = PlayFromTimelines(cur.getTaid());
            yield return new WaitForSeconds((float)currentTimeline.duration);
        }
        isPlaying = false;
    }

}



