using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using UnityEngine.Timeline;
using System.Collections.Generic;
namespace Hexiled.SoHi{
[CreateAssetMenu(fileName = "Node", menuName = "SoHi/Node")]
[Serializable]
public class Node:ScriptableObject 
{
	[HideInInspector]
	public bool showChild = true;
	[HideInInspector]
	public List<Node> children = new List<Node> ();

	[SerializeField]
	protected string intent;
	[SerializeField]
     protected string response;
	[SerializeField]
     protected int taid;

    

        public void AddChild<T>(T node) where T : Node{
		children.Add (node);
	}
		public void InsertChildAt<T>( T node,int index) where T:Node{
			children.Insert (index, node);
		}

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
}
