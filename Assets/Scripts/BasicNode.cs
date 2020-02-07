using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
[CreateAssetMenu]
//This is the base node from which all annotated nodes inherit from
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
        timelineController.Play(taid, response);
    }
    public string getIntent()
    {
        return intent; 
    }
}
