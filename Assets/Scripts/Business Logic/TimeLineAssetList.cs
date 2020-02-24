using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[CreateAssetMenu]
public class TimeLineAssetList : ScriptableObject
{
   public List<TimelineAsset> list = new List<TimelineAsset>();
}
