using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInstantiator : MonoBehaviour
{
    public GameObject LeftHandParent;
    public GameObject RightHandParent;

    private GameObject currentObject;
    private List<GameObject> releasedObjects;

    private void Start()
    {
        releasedObjects = new List<GameObject>();
    }

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

    public void CallDestroyObjects(float offset)
    {
        StartCoroutine(DestroyObjects(offset));
    }

    public void CallRelease(float offset)
    {
        StartCoroutine(ReleaseObject(offset));
    }

    private IEnumerator InstantiateObject(GameObject go, GameObject parent, float offset)
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

    private IEnumerator ReleaseObject(float offset)
    {
        yield return new WaitForSeconds(offset);
        if(currentObject != null)
        {
            currentObject.transform.parent = null;
        }
        releasedObjects.Add(currentObject);
        currentObject = null;
    }

    private IEnumerator DestroyObjects(float offset)
    {
        yield return new WaitForSeconds(offset);
        foreach(GameObject current in releasedObjects)
        {
            Destroy(current);
        }
    }
}
