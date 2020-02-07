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
        node = (BasicNode)soHiTree.GetRoot();
        node.Play(timelineController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayFeedback()
    {
       // allNodes = soHiTree.GetAll(node);
        //TODO: ASYNC FUNCTION TO PLAY child nodes 
   
    }
   
    public void ReturnQuery(QueryResult query)
    {
        string intent = query.intent.displayName; 
        BasicNode active = (BasicNode)ScriptableObject.CreateInstance<Node>();
        Node root = soHiTree.GetRoot();
        active = (BasicNode)soHiTree.MatchIntent(intent,root);
        active.Play(timelineController); 
    }
}
