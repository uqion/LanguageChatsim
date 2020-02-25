using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
//Transaction Node; calculate shopping cart total for $price
public class TransactionNode1 : Node
{
    public override void Play(Tree_Container tree, List<Node> nodelist)
    {
        Debug.Log("Reached TRANSACTION1 NODE PLAY");
        response = "The total is " + tree.GetBillTotal() + "dollars";
        Debug.Log("TOTAL IS:" + tree.GetBillTotal());
        tree.PlayChildren(nodelist);
        tree.shoppingCart.ResetCart();
    }
    

    public new void Play(Tree_Container tree)

    {
        Debug.Log("Reached TRANSACTION1 NODE PLAY");
        response = "The total is " + tree.GetBillTotal() + " dollars";
        Debug.Log("TOTAL IS:" +tree.GetBillTotal());
        tree.Play(this);
        tree.shoppingCart.ResetCart();
        
    }



}
