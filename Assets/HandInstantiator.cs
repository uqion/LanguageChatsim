using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInstantiator : MonoBehaviour
{
    public GameObject LeftHandParent;
    public GameObject RightHandParent;

    private GameObject currentObject;

    public void CallInstantiate(GameObject objectToInstantiate, float offset, bool isLeftHand)
    {
        Debug.Log("CALLED INSTANTIATE");
        if(isLeftHand)
        {
            InstantiateObject(objectToInstantiate, LeftHandParent, offset);
        } else
        {
            InstantiateObject(objectToInstantiate, RightHandParent, offset);
        }
    }

    public IEnumerator InstantiateObject(GameObject go, GameObject parent, float offset)
    {
        Debug.Log("HAND: Calling instantiate");
        yield return new WaitForSeconds(offset);
        if(currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
        Debug.Log("instantiating");
        Instantiate(go, parent.transform);
        currentObject = go;
    }
}
