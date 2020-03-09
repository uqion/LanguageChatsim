using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public SpeechManager speech;
    public AudioSource audioSource;

    private Queue<Node> queuedTimelines;
    private Queue<int> queuedTimelinesRoot;
    private bool isPlaying = false;

    private void Awake()
    {
        queuedTimelines = new Queue<Node>();
        queuedTimelinesRoot = new Queue<int>();
    }


    //aggregated play function with audio and animations
    //PARAM is taid passed from SO node, it is the index location of the timeline asset, string response to TTS
    //TODO SALSA integration 
    public async void Play(Node node)
    {
        string response = node.getResponse();
        int taid = node.getTaid();
        AudioClip clip = await speech.SpeakWithSDKPlugin(response);
        if(clip != null )
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        PlayFromTimelines(taid);
    }
    public void Play(int id)
    {
        PlayFromTimelines(id);
    }

    //Animations play method
    //PARAM is taid passed from SO node, it is the index location of the timeline asset 
    //sets off a single timeline to play asynchronously
    public TimelineAsset PlayFromTimelines(int index)
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
    /**public void PlayFromTimelines(List<Node> queue, Tree_Container tree)
  {
       if (isPlaying)
       {
           Debug.Log("tried to play timelines while there are others playing");
           return;
       }
       foreach (Node n in queue)
       {    
           queuedTimelines.Enqueue(n);
       }
       StartCoroutine(playQueue(tree));
   }**/
    public void PlayFromTimelines(params int[] queue)
    {
        if (isPlaying)
        {
            Debug.Log("tried to play timelines while there are others playing");
            return;
        }
        foreach (int n in queue)
        {
            queuedTimelinesRoot.Enqueue(n);
        }
        StartCoroutine(playQueueRoot());
    }

   /** public IEnumerator playQueue(Tree_Container tree)
    {
        isPlaying = true;
        while (queuedTimelines.Count > 0)
        {
            Node cur = queuedTimelines.Dequeue();
            Play(cur);
            TimelineAsset currentTimeline = PlayFromTimelines(cur.getTaid());

            yield return new WaitForSeconds((float)currentTimeline.duration);
        }
        isPlaying = false;
    }**/
    private IEnumerator playQueueRoot()
    {
        isPlaying = true;
        while (queuedTimelinesRoot.Count > 0)
        {
            int cur = queuedTimelinesRoot.Dequeue();
            Play(cur);
            TimelineAsset currentTimeline = PlayFromTimelines(cur);
            yield return new WaitForSeconds((float)currentTimeline.duration);
        }
        isPlaying = false;
    }
   

}



