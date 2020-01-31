using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
using JsonData;

public class Tree_Container : MonoBehaviour
{
    [SerializeField]
    SoHiTree soHiTree;
    [SerializeField]
    TimelineController timelineController;

    private BasicNode node;
    // Start is called before the first frame update
    void Start()
    {
        node = (BasicNode)soHiTree.getRoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playFeedback(QueryResult query)
    {
        //TODO: search for the correct node based on intent in query
        node.Play(timelineController);
    }
}
