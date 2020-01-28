using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
[CreateAssetMenu]
public class BasicNode : Node
{
    [SerializeField]
    public string Response;
    [SerializeField]
    TimelineController TimelineController;
    [SerializeField]
    public int taid;
    

    public void Play()
    {
        TimelineController.PLayFromTimelines(taid);
    }

}
