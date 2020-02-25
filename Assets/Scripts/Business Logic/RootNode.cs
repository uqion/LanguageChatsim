using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

//Root node of SO_Hi tree
//Random generation of P01 greeting prompts, leading to P02A1 bundle (Timeline Asset:'Nod Prompt')
//Greeting triggered by user proximity 
public class RootNode : Node
{

    public override void Play(Tree_Container tree)
    {
        tree.GetGreeting(); 
    }
}
