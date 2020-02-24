using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityEngine.Timeline;
using System.Collections.Generic;


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

    virtual public void Play(Tree_Container tree) {
        Debug.Log("Reached NODE PLAY");
        //decorated node logic + SINGLE
        tree.Play(this);//visitor pattern; double dispatch

    }
    //Overloading methods to accommodate nodes with children 

    virtual public void Play(Tree_Container tree, List<Node> nodelist) 
        {
            Debug.Log("Reached NODE PLAY");
        //decorated node logic + CHILDREN
            tree.PlayChildren(nodelist);//visitor pattern; double dispatch


    }


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

