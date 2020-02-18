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
    private Node node;
    private Queue<int> queuedTimelines;


    // Start is called before the first frame update
    void Start()
    {
        ReturnQuery("UserCorrectionWB");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayChild(Node node)
    {
        List<Node> queuedTimelines = node.children;
        queuedTimelines.Insert(0, node);
        timelineController.PlayFromTimelines(queuedTimelines);
       
    }
    public void ReturnQuery(string query)
    {
       
        
        Node active = ScriptableObject.CreateInstance<Node>();
        Node root = soHiTree.GetRoot();
        active = soHiTree.MatchIntent(query, root);
        if ((active.children).Any()){
      
        PlayChild(active);
          
        }
        else
        {
            active.Play(timelineController);
   
        }
    }


public void ReturnQuery(QueryResult query)
    {
        
        string intent = query.intent.displayName; 
        Node active = ScriptableObject.CreateInstance<Node>();
        Node root = soHiTree.GetRoot();
        active = soHiTree.MatchIntent(intent,root);
        active.Play(timelineController);
        if (!(active.children).Any())
        {
            PlayChild(active);
        }
    }
}
