using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFNode : Node
{
    // Start is called before the first frame update

    public override void Play(Tree_Container tree)
    {
        tree.apiScript.SendText(getIntent());//TODO: MATCH INTENT 
    }
}
