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
    public void PlayChild(BasicNode node)
    {
        int size = node.children.Count()+1;
        BasicNode[] queuedTimelines = new BasicNode[size];
        Debug.Log("NUMBER OF CHILDREN:" + size);
        for (int i = 0; i < node.children.Count(); i++)
        {
            
            BasicNode child = (BasicNode)node.children[i];
            queuedTimelines[i] = child;
           

            
        }
        queuedTimelines[0] = node;
        Debug.Log("NUMBER OF timelines:"+queuedTimelines.Count());
        for (int i = 0; i < queuedTimelines.Count(); i++)
        {
 
            Debug.Log("TAID is:"+queuedTimelines[i]);
        }
      
        timelineController.PlayFromTimelines(queuedTimelines);
        Debug.Log("REACHED HERE 6");
    }
    public void ReturnQuery(string query)
    {
        Debug.Log("REACHED HERE 1");
        //string intent = query.intent.displayName;
        BasicNode active = (BasicNode)ScriptableObject.CreateInstance<BasicNode>();
        BasicNode root = (BasicNode)soHiTree.GetRoot();
        active = soHiTree.MatchIntent(query, root);
        //Debug.Log(active.getIntent());
        Debug.Log("REACHED HERE 2");
       // Debug.Log("REACHED HERE 3");
        if ((active.children).Any()){
       Debug.Log("REACHED HERE 4");
        PlayChild(active);
           Debug.Log("REACHED HERE 5");
        }
        else
        {
            active.Play(timelineController);
            Debug.Log("REACHED HERE 6");
        }
    }


public void ReturnQuery(QueryResult query)
    {
        
        string intent = query.intent.displayName; 
        BasicNode active = (BasicNode)ScriptableObject.CreateInstance<Node>();
        BasicNode root = (BasicNode)soHiTree.GetRoot();
        active = (BasicNode)soHiTree.MatchIntent(intent,root);
        active.Play(timelineController);
        if (!(active.children).Any())
        {
            PlayChild(active);
        }
    }
}
