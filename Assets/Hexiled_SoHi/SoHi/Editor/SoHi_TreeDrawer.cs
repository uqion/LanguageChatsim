using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;

namespace Hexiled.SoHi
{
	[CustomPropertyDrawer (typeof(SoHiTree))]
	public class SoHi_TreeDrawer:PropertyDrawer
	{
		float DrawerHeight; //Since we take up space dinamically, we use this variable to store how much room we need.
		SerializedObject serializedTree; //The serialized version of the Scriptable Object Tree we have in our inspector.

		Node toDelete; //we store a Node to be deleted
		/// <summary>
		/// Override this method to get space in the inspector.
		/// </summary>
		/// <returns>The height of the inspector.</returns>
		/// <param name="property">The serialized Tree.</param>
		/// <param name="label">Label.</param>
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight (property, label) + DrawerHeight;
		}

		/// <summary>
		/// Sets the height of the drawer in the inspector.
		/// </summary>
		/// <param name="height">Height.</param>
		void SetDrawerHeight (float height)
		{
			this.DrawerHeight += height;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			DrawerHeight = 0; //We reste the hight, else it just keeps growing with every OnGui call.
			position.height = 16; //then we set up how high our rows will be

			int controlID = GUIUtility.GetControlID (FocusType.Passive);

			//We create and instance of the TreeGUInfo class where we will calculate and store a dictionary of 
			//our nodes and indent levels based on the tree structure
			var state = (TreeGUInfo)GUIUtility.GetStateObject (
				typeof(TreeGUInfo), 
				controlID);

			//If no tree is currently in the inspector slot, we bail out
			EditorGUI.PropertyField (position, property, label);
			if (property.objectReferenceValue == null) {
				return;}

			position.y += 20;
			//Create the editor for the tree serialized property 
			Editor e = Editor.CreateEditor (property.objectReferenceValue);
			serializedTree = e.serializedObject;

			//All tree have a root node. If there is nothing in it, we create a button that allows to create it
			var treeRoot = serializedTree.FindProperty ("root");
			if (treeRoot.objectReferenceValue == null) {
				SetDrawerHeight (20);
				Node rootSO = (Node)ScriptableObject.CreateInstance<Node> ();
				Undo.RegisterCreatedObjectUndo (rootSO, "Create Root");

				AssetDatabase.AddObjectToAsset (rootSO, property.objectReferenceValue);
				rootSO.name = "Root";
				treeRoot.objectReferenceValue = rootSO;
				serializedTree.ApplyModifiedProperties ();
				return;
			}
			Node node = (Node)serializedTree.FindProperty ("root").objectReferenceValue;

			//Saving the tree is really simple. We simply save the asset and refresh
			if (GUI.Button (position, "Save Tree")) {
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}

			//We can be sure that this node exists, because otherwise we would have drawn the button

			position.y += 20;

			//This is a recursive call that creates the dictionary of nodes and indent levels.
			state.setIndentedNodes (node);

			//Based on the number of open nodes we take space in the inspector
			SetDrawerHeight ((state.indentedNodes.Count+2) * 20);

			//Now wi draw each row (that represents each tree)
			foreach (var pair in state.indentedNodes) {
				Row (position, pair.Key, state.indentedNodes, e);
				position.y += 20;
			}

			//Delete out side the drawing loop
			if (toDelete != null) {
				foreach (var pair in state.indentedNodes) {
					if (pair.Key.children.Contains (toDelete)) {
						Node myParent = pair.Key;
						Undo.RegisterCompleteObjectUndo(myParent, "Delete Node");
						int deleteIndex = myParent.children.IndexOf (toDelete);
						myParent.children.RemoveAt (deleteIndex); //Remove from the list of children;
						List<Node> childrenList = TreeGUInfo.getMyChildrenList (toDelete); //Build a list of deep children of this node
						for (int i = childrenList.Count-1; i >= 0; i--) { //Aparently we need to go in reverse for the Undo to properly work
							Undo.DestroyObjectImmediate (childrenList [i]); //This removes the object from the serialized version
						}
						break;
					}
				}
				toDelete = null;
			}
			e.Repaint ();
		}

		/// <summary>
		/// Defines how to draw each row (which represents a node), and how to react to GUI events
		/// </summary>
		/// <param name="position">Position in the inspector.</param>
		/// <param name="node">Node to draw.</param>
		/// <param name="dict">Dictionary with the nodes and indent levels.</param>
		/// <param name="e">The editor we are using to inspect the tree.</param>
		public virtual void Row (Rect position, Node node, Dictionary<Node,int> dict, Editor e)
		{
			Color originalColor = GUI.color; //We store this to be able to do some feedback with colors

			Event currentEvent = Event.current;
			EventType currentEventType = currentEvent.type; //Get the current GUI event

			#region Draw //Define how to draw the row
			EditorGUI.indentLevel = dict [node];

			position.width = 100 + EditorGUI.indentLevel * 10; //The width of the row 

			Rect indentedRect = EditorGUI.IndentedRect (position);
			GUI.Box (indentedRect, GUIContent.none);    //Draw the box

			//Make each node react to the position of the mouse
			if (position.Contains (currentEvent.mousePosition)){
				Color col = new Color(GUI.color.r,GUI.color.g,GUI.color.b,.7f);
				GUI.color =col;
			}

			EditorGUI.LabelField (position,node.name); //Put the name of the Node on top of the box.

			//Implement the Delete Button
			if (dict [node] != 0) {
				if (GUI.Button (new Rect (position.x + 100 + EditorGUI.indentLevel * 10, position.y, 16, 16), "-")) {
					toDelete = node; //We simply set the reference to delete after the draw loop
					return;

				}
			}

			//The width of the foldout arrow
			position.width = 10 + EditorGUI.indentLevel * 10;
			//Draw the foldout arrow last so it's the first thing that mouse click interact with.
			if (node.children.Count != 0) {
				node.showChild = EditorGUI.Foldout (position, node.showChild, GUIContent.none);
			}
			position.width = 100 + EditorGUI.indentLevel * 10;
			GUI.color = originalColor; //If the mouse leaves the node, it should get it's original color

			#endregion

			#region Events
			//If we just released a Node on the inspector, reset the Drag and Drop Information
			if (currentEventType == EventType.DragExited) {
				DragAndDrop.PrepareStartDrag ();
			}

			//If the mouse is outside this property drawer, we shouldnt care about the GUI events any more,
			// so we bail out early.
			if (!position.Contains (currentEvent.mousePosition))
				return;

			switch (currentEventType) {
			//If we click on a row, select that node, and ping it's location in the project
			case EventType.MouseDown:

				Node nodeClicked = node;
				EditorGUIUtility.PingObject (nodeClicked);
				Selection.activeObject = nodeClicked;
				currentEvent.Use ();
				break;
			//If we start draging a node, we set the data of the node and star the Drag
			case EventType.MouseDrag:
				CustomDragData existingDragData = DragAndDrop.GetGenericData ("currentValue") as CustomDragData;
				if (existingDragData == null ) {
					DragAndDrop.PrepareStartDrag ();

					// reset data
					CustomDragData dragData = new CustomDragData ();
					dragData.origin = node;
					//We need to find the node parent because if we end up accepting this drag, 
					//we will delete this node from it's parents Children list.
					foreach (var pair in dict) { 
						Node possibleParent = pair.Key;
						if (possibleParent.children.Contains (node)) {
							dragData.parent = possibleParent; //Store the parent in the Drag and Drop data
							break;
						}
					}
					DragAndDrop.SetGenericData ("currentValue", dragData);
					UnityEngine.Object[] objectReferences = new UnityEngine.Object[1]{ node };
					DragAndDrop.objectReferences = objectReferences;
					DragAndDrop.StartDrag (node.name);
					currentEvent.Use ();
				}
				break;
			
			//Change the way the mouse pointer looks
			case EventType.DragUpdated:
				if (DragAndDrop.objectReferences [0] is Node){
					DragAndDrop.visualMode = DragAndDropVisualMode.Link;}
				else{
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
				}
				currentEvent.Use ();
				break; 

			case EventType.Repaint:
				//If we are traying to move it somewhere we shouldnt, we dont repaint anything
				if (DragAndDrop.visualMode == DragAndDropVisualMode.None ||
					DragAndDrop.visualMode == DragAndDropVisualMode.Rejected)
					break;

				//Now, this is hacky. I'm sure there is a better way to solve this, but...
				//We paint a diferent Rect depending on weather we are trying to 
				//add a node as a child (right on top of the current node) or as a
				//sibling (under the current node)
				Rect _addAsChild = new Rect (indentedRect.x, indentedRect.y, indentedRect.width, 12);
				if (_addAsChild.Contains (currentEvent.mousePosition)) {
					Color myColor = Color.grey;
					myColor.a = .3f;
					EditorGUI.DrawRect (_addAsChild, myColor);
				}
				Rect _addAsSibling = new Rect (indentedRect.x, indentedRect.y + 12, indentedRect.width, 8);
				if (_addAsSibling.Contains (currentEvent.mousePosition) && dict [node] != 0) {
					Color myColor = Color.blue;
					myColor.a = .3f;
					EditorGUI.DrawRect (_addAsSibling, myColor);
				}

				break;

				//Perform the operations on the Children List of the appropiate nodes to either add a new node,
				//or change and existing node location
			case EventType.DragPerform:
				Node target;
				CustomDragData originalData = DragAndDrop.GetGenericData ("currentValue") as CustomDragData;
				Node parent;
				_addAsChild = new Rect (indentedRect.x, indentedRect.y, indentedRect.width, 12);
				if (_addAsChild.Contains (currentEvent.mousePosition)) {
					target = node;
					//If there is no original data it means that the drag started somewhere outside the 
					//property drawer, so we will create a new node.
					if (originalData == null) {
						Node origin = DragAndDrop.objectReferences [0]as Node;
						Node newChild =TreeGUInfo.AddChildrenRecursive(origin, AssetDatabase.GetAssetPath (serializedTree.targetObject));
						Undo.RegisterCreatedObjectUndo(newChild,"Add Child");
						Undo.RegisterCompleteObjectUndo(target,"Add Child"); 

						target.AddChild (newChild);
						DragAndDrop.AcceptDrag ();
						GUI.changed = true;
						Event.current.Use ();
						break;

					} else {
						//If there is original data we add the node to the new parent and delete it from the previous.
						Node origin = originalData.origin;

						parent = originalData.parent;
						if (parent != target && target != originalData.origin && !TreeGUInfo.getMyChildrenList(origin).Contains(node)) {
							Undo.RegisterCompleteObjectUndo(parent,"Change Parent");
							Undo.RegisterCompleteObjectUndo(node,"Change Parent");

							parent.children.RemoveAt (parent.children.IndexOf (origin));
							node.AddChild (origin);
						}
					}
					DragAndDrop.AcceptDrag ();
					GUI.changed = true;
					Event.current.Use ();
					break;
				}
				//we do the samething but adding as a sibling
				_addAsSibling = new Rect (indentedRect.x, indentedRect.y + 12, indentedRect.width, 8);
				if (_addAsSibling.Contains (currentEvent.mousePosition) && dict [node] != 0) {

					target = node;

					Node targetParent = null;
					foreach (Node targetParentNode in dict.Keys) {
						if (targetParentNode.children.Contains (node)) {
							targetParent = targetParentNode;
							break;
						} 
					}

					if (originalData != null) {
						if (node != originalData.origin) {
							Node origin = originalData.origin;
							parent = originalData.parent;
							SerializedObject parentSO = new SerializedObject (parent);
							int index = targetParent.children.IndexOf(node);
							if (origin != target && !TreeGUInfo.getMyChildrenList(origin).Contains(node)) {
								Undo.RegisterCompleteObjectUndo(parent,"Change Siblings");
								Undo.RegisterCompleteObjectUndo(targetParent,"Change Siblings");
								parentSO.ApplyModifiedProperties ();
								parentSO.Update ();
								int i = index+1;
								targetParent.InsertChildAt(origin,i);
								parent.children.RemoveAt (parent.children.IndexOf (origin));
							}

							GUI.changed = true;
							DragAndDrop.AcceptDrag ();
							Event.current.Use ();

						} 
					} else {

						Node origin = DragAndDrop.objectReferences [0] as Node;
						var myType = origin.GetType ();
						Node newChild = (Node)ScriptableObject.CreateInstance (myType);
						Undo.RegisterCreatedObjectUndo(newChild,"Add Sibling");
						Undo.RegisterCompleteObjectUndo(targetParent,"Add Sibling"); 

						EditorUtility.CopySerialized (origin, newChild);

						int index = targetParent.children.IndexOf(node);
						targetParent.InsertChildAt(newChild,index+1);
						AssetDatabase.AddObjectToAsset (newChild, AssetDatabase.GetAssetPath (serializedTree.targetObject));
						DragAndDrop.AcceptDrag ();
						GUI.changed = true;
						Event.current.Use ();
					}
				}
				Event.current.Use ();
				break;

			case EventType.MouseUp:
				e.Repaint();
				Event.current.Use ();
				break;
			}
			#endregion
		}


	}

	//The class we use to store Drag And Drop Information
	public class CustomDragData
	{
		public Node origin;
		public Node parent;
	}
}

