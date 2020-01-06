using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dispatcher : MonoBehaviour
{
    Queue<Action> actionQueue = new Queue<Action>();
    
    // Update is called once per frame
    void Update()
    {
        if(actionQueue.Count > 0)
        {
            actionQueue.Dequeue().Invoke();
        }
    }

    public void AddTask(Action actionToAdd)
    {
        actionQueue.Enqueue(actionToAdd);
    }
}
