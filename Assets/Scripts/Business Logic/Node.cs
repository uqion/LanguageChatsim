using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityEngine.Timeline;
using System.Collections.Generic;
using UnityEngine.Playables;

[CreateAssetMenu]
[Serializable]
public class Node : ScriptableObject
{
    [SerializeField]
    protected string intent;
    [SerializeField]
    protected string response;
    [SerializeField]
    protected int taid;
    [SerializeField]
    protected string[] auxilliaryAnimations;
    [SerializeField]
    protected float[] auxilliaryOffsets;

    virtual public void Play(Tree_Container treeContainer) {
        Debug.Log("Reached NODE PLAY");
        if(auxilliaryAnimations != null && auxilliaryOffsets != null && auxilliaryOffsets.Length == auxilliaryAnimations.Length)
        {
            AuxilliaryAnimator animator = GameObject.Find("Systems").GetComponent<AuxilliaryAnimator>();
            for(int i = 0; i < auxilliaryAnimations.Length; i++)
            {
                GameObject g = GameObject.Find(auxilliaryAnimations[i]);
                if(g != null)
                {
                    animator.CallAnimation(g.GetComponent<PlayableDirector>(), auxilliaryOffsets[i]);
                }
            }
        }
        //decorated node logic + SINGLE
       // treeContainer.timelineController.Play(this);//visitor pattern; double dispatch

    }
   
    
   // virtual public void Play(Tree_Container tree, List<Node> nodelist
    public string getIntent()
    {
        return intent;
    }
    public int getTaid()
    {
        return taid;
    }
    public string getResponse()
    {
        return response;
    }
}

