using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Hexiled.SoHi;
[CustomEditor (typeof(myNode))]

public class SelectorPropertyDrawer : Editor {

	public override void OnInspectorGUI()
	{
		target.name = EditorGUILayout.TextField (target.name);

		myNode m_node = (myNode)target;
		DrawDefaultInspector ();
		if(GUILayout.Button("Introduce yer self"))
		{
			m_node.SayHello ();
		}
	}
}
