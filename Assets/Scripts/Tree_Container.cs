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
        ReturnQuery("testnode");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void PlayChild(BasicNode node)
    {

        for (int i = 0; i < node.children.Count; i++)
        {
           
            BasicNode child = (BasicNode)node.children[i];
            await child.Play(timelineController);
            Debug.Log("NUMBER OF CHILDREN:" + node.children.Count());
        }
    }
    public async void ReturnQuery(string query)
    {
        Debug.Log("REACHED HERE 1");
        //string intent = query.intent.displayName;
        BasicNode active = (BasicNode)ScriptableObject.CreateInstance<BasicNode>();
        BasicNode root = (BasicNode)soHiTree.GetRoot();
        active = soHiTree.MatchIntent(query, root);
        //Debug.Log(active.getIntent());
        Debug.Log("REACHED HERE 2");
      await active.Play(timelineController);
       // Debug.Log("REACHED HERE 3");
        if ((active.children).Any()){
       Debug.Log("REACHED HERE 4");
        PlayChild(active);
           Debug.Log("REACHED HERE 5");
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
