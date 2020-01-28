using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline; 
[CreateAssetMenu]
public class TimelineController : ScriptableObject
{
    [SerializeField]
    public PlayableDirector playableDirector;
    [SerializeField]
    public List<TimelineAsset> timelines;

    public void Play()
    {
        playableDirector.Play();
        }
    public void PLayFromTimelines(int index)
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



