using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hexiled.SoHi;
namespace Hexiled.SoHi{
//this class stores the dictionary we use to draw each Row of our tree.
public class TreeGUInfo {
		
	public Dictionary<Node,int> indentedNodes = new Dictionary<Node, int> (); //This is the dictionary we need.
	public List<Node> allNodes= new List<Node> ();  //this are all the nodes, not just the ones currently open
	static List<Node> childNodes= new List<Node> ();

		/// <summary>
		/// We start the recursive call to build the dictionary.
		/// </summary>
		/// <param name="node">Node.</param>
	public void setIndentedNodes(Node node){
		indentedNodes = new Dictionary<Node, int> ();//We build the dictionary from scratch every OnGUI call
													//to handle the opening and closing of the foldouts.
													//This is probably not efficient at all. Is there a smarter way 
													//to handle only the changes???

		SetNodesRecursive (node,0);  //Start the recursive call
	}
	void SetNodesRecursive(Node node,int index){
		indentedNodes.Add (node, index);
		//Here we handle if the foldouts are open or closed
		if (!node.showChild || node.children.Count==0)
			return;
		index++;
		for (int i = 0; i < node.children.Count; i++) {
			Node child = node.children[i];
			SetNodesRecursive (child, index);
		}
	}
	
		//If we want the whole list of nodes, we still need to traverse the tree recursively
	public void getAllNodes(Node node){
		allNodes = new List<Node> ();
		getAllNodesRecursive (node);  //recursive call** 
	}

	
	public void getAllNodesRecursive(Node node){
		allNodes.Add (node);
		for (int i = 0; i < node.children.Count; i++) {
			Node child = node.children[i];
			getAllNodesRecursive(child);
		}
	}
		//If we duplicate a tree, we need to reconstruct it recursively
		public static Node AddChildrenRecursive(Node node,string path ){
			var myType = node.GetType ();
			Node newNode = (Node)ScriptableObject.CreateInstance (myType);
			Undo.RegisterCreatedObjectUndo(newNode,"Add Child");
			EditorUtility.CopySerialized (node, newNode);

			newNode.children = new List<Node> ();
			foreach (Node child in node.children) {
				Node newChild = AddChildrenRecursive (child, path);
				newNode.AddChild (newChild);
			}
			AssetDatabase.AddObjectToAsset (newNode, path);
			return newNode;
		}



	//Just in case we need to manually reset the dictionary
	public void ResetDict(){
		indentedNodes = new Dictionary<Node, int> ();
	}

		/// <summary>
		/// Gets my children list is helpfull to create a list of every node that is a deep child of a specific node
		/// </summary>
		/// <returns>The my children list.</returns>
		/// <param name="node">Node.</param>
		public static List<Node> getMyChildrenList(Node node){
			childNodes = new List<Node> ();
			listOfChildren(node);
			return childNodes;
		}
		/// <summary>
		/// Recursively get the list of deep children
		/// </summary>
		/// <param name="node">Node.</param>
	 static void listOfChildren(Node node){
			
			if (node.children.Count == 0) {
				childNodes.Add (node);
				return;
			}
			foreach (var c in node.children) {
				listOfChildren (c);
			}
			childNodes.Add (node);
			return;
		}
}
}