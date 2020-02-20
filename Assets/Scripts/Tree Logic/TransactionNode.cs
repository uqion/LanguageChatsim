using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;

[CreateAssetMenu]
//call to SO DB & shopping cart 
public class TransactionNode : Node
{
   public new void Play(Tree_Container tree)
    {
        string item = tree.getIntent();
        tree.MakePurchase(item);
        tree.Play(this); 
    }

 

}

