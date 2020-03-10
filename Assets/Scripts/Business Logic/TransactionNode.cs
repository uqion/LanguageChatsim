using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Node", menuName = "Transaction Node")]
//call to SO DB & shopping cart 
public class TransactionNode : Node
{

    public float InstantiateOffset;
    public float ReleaseOffset;
    public GameObject ObjectToInstantiate;
    public bool isLeftHand;

    public override void Play(Tree_Container tree)
    {
        Debug.Log("Reached TRANSACTION NODE PLAY");
        string item = tree.getIntent();
        tree.MakePurchase(item);
        Debug.Log("HAND: " + InstantiateOffset);
        if (ObjectToInstantiate != null)
        {
            HandInstantiator instantiator = GameObject.Find("Systems").GetComponent<HandInstantiator>();
            instantiator.CallInstantiate(ObjectToInstantiate, InstantiateOffset, isLeftHand);
            instantiator.CallRelease(ReleaseOffset);

        }
        Debug.Log("SHOPPING CART CALL");
        base.Play(tree);
       
    }

}

