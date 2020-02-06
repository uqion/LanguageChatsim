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

		public Node getRoot()
		{
				return root;	
		}
		public List<Node> getAll(Node node)
		{
			allNodes = new List<Node>();

			getAllNodesRecursive(node);  //recursive call** 
			return allNodes;
		}
		//Helper method to getAll to recursively search tree
		public void getAllNodesRecursive(Node node)
		{
			allNodes.Add(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				Node child = node.children[i];
				getAllNodesRecursive(child);
			}
		}

	}
}

	

