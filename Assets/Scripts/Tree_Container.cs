using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
using JsonData;
using System.Linq;
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
       //node = (BasicNode)soHiTree.GetRoot();
        //node.Play(timelineController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayChild(BasicNode node)
    {

        for (int i = 0; i < node.children.Count; i++)
        {
            BasicNode child = (BasicNode)node.children[i];
            child.Play(timelineController);
            }
    }
   
    public void ReturnQuery(QueryResult query)
    {
        string intent = query.intent.displayName; 
        BasicNode active = (BasicNode)ScriptableObject.CreateInstance<Node>();
        BasicNode root = (BasicNode)soHiTree.GetRoot();
        active = (BasicNode)soHiTree.MatchIntent(intent,root);
        Debug.Log("REACHED HERE");
        Debug.Log(root.getIntent());
        active.Play(timelineController);
        if (!(active.children).Any())
        {
            PlayChild(active);
        }
    }
}
