using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class NodeList 
{
    public List<Node> Nodes; 

    public List<Node> getList()
    {
        return Nodes; 
    }
}
