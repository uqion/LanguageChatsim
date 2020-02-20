using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
[CreateAssetMenu]

//Root node of SO_Hi tree
//Random generation of P01 greeting prompts, leading to P02A1 bundle (Timeline Asset:'Nod Prompt')
//Greeting triggered by user proximity 
public class RootNode : Node
{

    public IEnumerator GetGreeting(TimelineController timeline)
    {
        int greeting = Random.Range(0, 3);
        Debug.Log("calling greeting " + greeting);
        if (greeting == 0)
        {
            timeline.PlayFromTimelines(0, 1, 5);
            yield return null;
        }
        else if (greeting == 1)
        {
            timeline.PlayFromTimelines(2, 3, 5);
            yield return null;
        }
        else if (greeting == 2)
        {
            timeline.PlayFromTimelines(4, 5);
            yield return null;
        }
    }
}
