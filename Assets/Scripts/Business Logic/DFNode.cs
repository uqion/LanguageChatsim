using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DFNode : Node { 
    [SerializeField]
    public string DFTrigger;

    // Start is called before the first frame update

    public override void Play(Tree_Container tree)
    {
        tree.apiScript.SendText(DFTrigger);//TODO: MATCH INTENT 
    }
}
