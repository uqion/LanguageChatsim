using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;

[CreateAssetMenu]
//Transaction Node; calculate shopping cart total for $price
public class TransactionNode1 : Node
{
    public new void Play(Tree_Container tree)
    {
        response = "The total is" + tree.GetBillTotal();
        tree.Play(this);
    }



}
