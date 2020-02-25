using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
//Transaction Node; call to check if no.items >1, if yes, jump directly to P10_A4, empty cart3
public class TransactionNode2 : Node
{
    public new void Play(Tree_Container tree)
    {
        if (tree.CartHasItem())
        {
            taid = 22;
        }
        else
        {
            taid = 20; 
        }
        tree.Play(this); 
    }



}
