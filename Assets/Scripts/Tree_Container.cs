using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi; 

public class Tree_Container : MonoBehaviour
{
    [SerializeField]
    SoHiTree soHiTree;
    [SerializeField]
    TimelineController timelineController;
    // Start is called before the first frame update
    void Start()
    {
        soHiTree.getRoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
