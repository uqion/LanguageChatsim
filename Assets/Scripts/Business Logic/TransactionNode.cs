using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Node", menuName = "Transaction Node")]
//call to SO DB & shopping cart 
public class TransactionNode : Node
{

    public float InstantiateOffset;
    public GameObject ObjectToInstantiate;

    public override void Play(Tree_Container tree)
    {
        Debug.Log("Reached TRANSACTION NODE PLAY");
        string item = tree.getIntent();
        tree.MakePurchase(item);
        Debug.Log("HAND: " + InstantiateOffset);
        if (ObjectToInstantiate != null)
        {
            GameObject.Find("Systems").GetComponent<HandInstantiator>().CallInstantiate(ObjectToInstantiate, InstantiateOffset, false);
        }
        Debug.Log("SHOPPING CART CALL");
        tree.timelineController.Play(this);
       
    }

}

