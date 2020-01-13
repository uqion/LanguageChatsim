using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class can be used when inside of a thread to interact with Unity components
/// by passing an action to AddTask
/// </summary>
public class Dispatcher : MonoBehaviour
{
    /// <summary>
    /// This queue is used to store the actions that Dispatcher will invoke
    /// </summary>
    Queue<Action> actionQueue = new Queue<Action>();
    
    /// <summary>
    /// On every frame, one action will be executed if the queue is not empty
    /// </summary>
    void Update()
    {
        if(actionQueue.Count > 0)
        {
            actionQueue.Dequeue().Invoke();
        }
    }

    /// <summary>
    /// Action passed into this function will be invoked by Dispatcher's update function
    /// </summary>
    /// <param name="actionToAdd">action to be invoked</param>
    public void AddTask(Action actionToAdd)
    {
        actionQueue.Enqueue(actionToAdd);
    }
}
