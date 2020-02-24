using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JsonData;
using System.Linq;


//Contains an instantiated SO_Hi tree; interface for Timeline Controller 
public class Tree_Container : MonoBehaviour
{
    

   
    [SerializeField]
    public List<Node> nodeTree;
    [SerializeField]
    private NodeDictionary nodeDictionary;



    [SerializeField]
    TimelineController timelineController;
    [SerializeField]
    SO_Database database;
    [SerializeField]
    ShoppingCart shoppingCart; 
   
    private Queue<int> queuedTimelines;
    [SerializeField]
    RootNode rootNode;
    private bool isColliding;
    private string intent; 

    // Start is called before the first frame update
    void Start()
    {//TODO: rootnode flag
     // tree = new Dictionary<string, List<Node>>();
     //ReturnQuery("DefaultWelcome");
     ReturnQuery("UserProvidesBeverageRight");
     //ReturnQuery("UserProvidesBeverageRight");
     //  ReturnQuery("UserProvidesBeverageRight - no");
     // rootNode = ScriptableObject.CreateInstance<RootNode>();
    }

    // Update is called once per frame
    void Update()
    {

    }
   
   


   // public void PlayChild(Node node)//TODO: implement visitor pattern for nodes with children 
    //{
      //  node.Play(this); 
       //List<int> queuedTimelines;
        //if (!queuedTimelines[0].Equals(node))
        //{
           // queuedTimelines.Insert(0, node);
        //}
        //timelineController.PlayFromTimelines(queuedTimelines);

   // }
   // public List<Node> PopulateQueue(Node node)
  //  {
       // List<Node> children = new List<Node>();
       // for(int i=0; i<node.children.Count(); i++)
        //{
          //  children[i] = nodeTree[node.children[i]];
      // }
       // return children; 
  //  }
    public void ReturnQuery(string query)
    {


        Node active;
        intent = query; 
       active = MatchIntent(query);
        active.Play(this);
    
    }
    public Node MatchIntent(string intent)


    {
        return nodeDictionary[intent];
       
    }

    public void ReturnQuery(QueryResult query)
    {
        Debug.Log("REACHED HERE");
        intent = query.intent.displayName;
        Debug.Log("The df INTENT IS:" + intent);
        Node active = MatchIntent(intent);
        Debug.Log("The MATCHED INTENT IS:" +active.getIntent());
        active.Play(this);




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