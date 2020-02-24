using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
//Transaction Node; calculate shopping cart total for $price
public class TransactionNode1 : Node
{
    public new void Play(Tree_Container tree)
    {
        response = "The total is" + tree.GetBillTotal();
        if (children.Count > 0)
        {
          //  tree.PlayChild(this);
        }
        else
        {
            tree.Play(this);
        }
    }



}
