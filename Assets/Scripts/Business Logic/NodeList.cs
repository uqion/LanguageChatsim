using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
//Custom NodeList class that allows List<Node> to be serialized to be used as a value in NodeDictionary
public class NodeList 
{
    public List<Node> Nodes;

  
    public List<Node> getList()
    {
        return Nodes; 
    }
}
