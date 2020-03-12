using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateItemLabel : MonoBehaviour
{

    public GameObject itemLabel;
    public GameObject tintSphere;

    // Start is called before the first frame update
    void Start()
    {
        //itemLabel.SetActive(false);
    }


    private void OnTriggerStay(Collider other)
    {
    
        if (tintSphere.activeSelf)
        {
            itemLabel.SetActive(true);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
            itemLabel.SetActive(false);
    }

}
