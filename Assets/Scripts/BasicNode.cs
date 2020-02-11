using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
using System.Threading.Tasks;

[CreateAssetMenu]
//This is the base node from which all annotated nodes inherit from
public class BasicNode : Node
{
    [SerializeField]
    string intent;
    [SerializeField]
    string context;
    [SerializeField]
    string response;
    [SerializeField]
    int taid;
   
    //no audio
    public Task Play(TimelineController timelineController)
    {
        return new Task(() =>
        { if (string.IsNullOrEmpty(response))
        {
            timelineController.Play(taid);
        }
        else
        {
            timelineController.Play(taid, response);
        }});
    }
    
    public string getIntent()
    {
        return intent; 
    }
    public int getTaid()
    {
        return taid; 
    }

}
