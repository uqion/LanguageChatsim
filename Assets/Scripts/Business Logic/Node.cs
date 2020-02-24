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



     

        virtual public void Play(Tree_Container tree) //visitor pattern; double dispatch

        {
            Debug.Log("Reached NODE PLAY");
       
            tree.Play(this);
        
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

