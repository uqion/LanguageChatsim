using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hexiled.SoHi;

[CreateAssetMenu(fileName = "myNode", menuName = "SoHi/Examples/myNode")]
public class myNode : Node {

	public string myString = "This is just a string";

	public void SayHello(){
		Debug.Log ("My name is: "+this.name);
		Debug.Log ("My String is: " + myString);
	}
}
