using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Hexiled.SoHi
{
	[CreateAssetMenu(fileName = "Tree", menuName = "SoHi/Tree")]
	public class SoHiTree : Node
	{
		[SerializeField]
		Node root;
		private List<Node> allNodes;

		public Node GetRoot()
		{		
				return root;	

		}
	
		//recursively traverses tree to get a list of all of tree's nodes 
		public List<Node> GetAllNodesRecursive(Node node)
		{
			
			allNodes.Add(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				Node child = node.children[i];
				GetAllNodesRecursive(child);
			}
			return allNodes;
	}
		//recursively traverses tree to match intent
		public Node MatchIntent(string intent, Node node)
		{
			Node child = ScriptableObject.CreateInstance<Node>();
			for (int i = 0; i < node.children.Count; i++)
			{
				child = node.children[i];
				if (string.Compare(child.getIntent(), intent) == 0)
				{
					return child; 
				}
				MatchIntent(intent,child);
			}
			return child; 
		}
        
    }

}

	

