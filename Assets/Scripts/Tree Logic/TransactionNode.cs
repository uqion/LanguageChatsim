using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;

[CreateAssetMenu(fileName = "Node", menuName = "Transaction Node")]
//call to SO DB & shopping cart 
public class TransactionNode : Node
{
   public override void Play(Tree_Container tree)
    {
        Debug.Log("Reached TRANSACTION NODE PLAY");
        string item = tree.getIntent();
        tree.MakePurchase(item);
        Debug.Log("SHOPPING CART CALL");
        tree.Play(this); 
    }

 

}

