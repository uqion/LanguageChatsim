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
		
		public Node Root
		{
			get
			{
				return root;
			}
			protected set
			{
				root = value;
			}
		}
		
	}
}

	

