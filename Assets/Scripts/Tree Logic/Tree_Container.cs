using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;
using JsonData;
using System.Linq;

//Contains an instantiated SO_Hi tree; interface for Timeline Controller 
public class Tree_Container : Container
{
    
    [SerializeField]
    TimelineController timelineController;
    [SerializeField]
    SO_Database database;
    [SerializeField]
    ShoppingCart shoppingCart; 
    private List<Node> allNodes = new List<Node>();
    private Node node;
    private Queue<int> queuedTimelines;
    private RootNode rootNode;
    private bool isColliding;
    private string intent; 

    // Start is called before the first frame update
    void Start()
    {//TODO: rootnode flag

        //ReturnQuery("DefaultWelcome");
       // ReturnQuery("UserProvidesWrongInfoWB");
        ReturnQuery("UserProvidesBeverageRight");
        //ReturnQuery("UserProvidesBeverageRight - no");
        // rootNode = ScriptableObject.CreateInstance<RootNode>();
    }

    // Update is called once per frame
    void Update()
    {

    }
   
   


    public void PlayChild(Node node)//TODO: implement visitor pattern for nodes with children 
    {
        List<Node> queuedTimelines = node.children;
        if (!queuedTimelines[0].Equals(node))
        {
            queuedTimelines.Insert(0, node);
        }
        timelineController.PlayFromTimelines(queuedTimelines);

    }
    public void ReturnQuery(string query)
    {


        Node active;
        Node root = tree.GetRoot();
        intent = query; 
        active = tree.MatchIntent(query, root);
        if ((active.children).Any())
        {
            PlayChild(active);
        }
        else
        {
            active.Play(this);
        }
    }


    public void ReturnQuery(QueryResult query)
    {
        Debug.Log("REACHED HERE");
        intent = query.intent.displayName;
        Debug.Log("THE MATCHED INTENT IS:" + intent);
        Node active = ScriptableObject.CreateInstance<Node>();
        Node root = tree.GetRoot();
        active = tree.MatchIntent(intent, root);

        if ((active.children).Any())
        {
            PlayChild(active);//children means not a decorated node? 
        }
        else
        {
            active.Play(this); //visitor pattern; double dispatch
        }
    }
    public void Play(Node node)

    {
        Debug.Log("Reached CONTAINER PLAY");
        string response = node.getResponse();
        int taid = node.getTaid();
        Debug.Log("RESPONSE IS"+response);
        if (string.IsNullOrEmpty(response))
        {
            Debug.Log("Reached ANIMATIONS PLAY");
            timelineController.Play(taid);
        }
        else
        {
            Debug.Log("Reached response PLAY");
            timelineController.Play(taid, response);
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Debug.Log(collider.gameObject.name);
            StartCoroutine(GetGreeting());
        }
    }
    public IEnumerator GetGreeting()
    {
        int greeting = Random.Range(0, 3);
        Debug.Log("calling greeting " + greeting);
        if (greeting == 0)
        {
            timelineController.PlayFromTimelines(0, 1, 5);
            yield return null;
        }
        else if (greeting == 1)
        {
            timelineController.PlayFromTimelines(2, 3, 5);
            yield return null;
        }
        else if (greeting == 2)
        {
            timelineController.PlayFromTimelines(4, 5);
            yield return null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
    }


    // adds item price into the bill.
    public void MakePurchase(string item)
    {
        shoppingCart.AddItem(database.GetPrice(item));
        Debug.Log("added" + database.GetPrice(item) + "to your cart.");
    }

    // Returns true if the shopping cart has atleast 1 item.
    public bool CartHasItem()
    {
        bool cartHasItem = false;
        if (shoppingCart.GetTotal() > 0)
        {
            cartHasItem = true;
        }
        return cartHasItem;
    }

    //Returns the total value of the bill
    public double GetBillTotal()
    {
        return shoppingCart.GetTotal();
    }

    public string getIntent()
    {
        return intent; 
    }
}