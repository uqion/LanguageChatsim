using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
[CreateAssetMenu]
public class BasicNode : Node
{
    [SerializeField]
    string intent; 
    [SerializeField]
    string response;
    [SerializeField]
    int taid;

    public void Play(TimelineController timelineController)
    {
        timelineController.PLayFromTimelines(taid);
    }




}
