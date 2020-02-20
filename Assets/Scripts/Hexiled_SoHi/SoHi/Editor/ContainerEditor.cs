using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Hexiled.SoHi;

[CustomEditor (typeof(Container))]
public class ContainerEditor : Editor {

	public override void OnInspectorGUI (){
			SerializedObject so = new SerializedObject (target);
			SerializedProperty treeProp = so.FindProperty ("tree");
			EditorGUILayout.PropertyField (treeProp);
			so.ApplyModifiedProperties ();
	}
}
