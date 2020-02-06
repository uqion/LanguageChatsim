using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
using JsonData;
//Contains an instantiated SO_Hi tree; interface for Timeline Controller 
public class Tree_Container : MonoBehaviour
{
    [SerializeField]
    SoHiTree soHiTree;
    [SerializeField]
    TimelineController timelineController;
    private List<Node> allNodes = new List<Node>();
    private BasicNode node;

    // Start is called before the first frame update
    void Start()
    {
        node = (BasicNode)soHiTree.getRoot();
        node.Play(timelineController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Node matchIntent(QueryResult query)
    {
        BasicNode active = (BasicNode)new Node(); 
        allNodes = soHiTree.getAll(node);
        return active; 
        
    }
}
