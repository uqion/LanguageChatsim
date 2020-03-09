using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
//Transaction Node; calculate shopping cart total for $price
public class TransactionNode1 : Node
{
    public float InstantiateOffset;
    public GameObject ObjectToInstantiate;

    public new void Play(Tree_Container tree)

    {
        Debug.Log("Reached TRANSACTION1 NODE PLAY");
        response = "The total is " + tree.GetBillTotal() + " dollars";
        Debug.Log("TOTAL IS:" +tree.GetBillTotal());
        tree.timelineController.Play(this);
        tree.shoppingCart.ResetCart();
        
    }



}
