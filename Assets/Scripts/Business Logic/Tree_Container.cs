using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JsonData;
using System.Linq;
using UnityEngine.Playables;
using UnityEngine.Timeline;


//Contains an instantiated NodeDictionary; interface for Timeline Controller 
public class Tree_Container : MonoBehaviour
{
   
    [SerializeField]
    public NodeDictionary nodeDictionary;
    [SerializeField]
    public TimelineController timelineController;
    [SerializeField]
    SO_Database database;
    [SerializeField]
    public ShoppingCart shoppingCart;
    [SerializeField]
    RootNode rootNode;
    private Queue<Node> queuedTimelines;
    
    private string intent;
    private bool isPlaying = false;



    // Start is called before the first frame update
    void Start()
    {//TODO: ROOTNODE FLAG 
     //TODO: ANIMATIONS DEFAULT POSITION
    //   ReturnQuery("DefaultFallback");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        queuedTimelines = new Queue<Node>();
       
    }
    //Overloaded ReturnQuery for testing without DialogFlow trigger
    public void ReturnQuery(string query)
    {
        Debug.Log("REACHED HERE");
        intent = query;
        NodeList active = MatchIntent(intent);
        if (active == null)//If intent from DF is not matched with keys in dictionary 
        {
            //ReturnQuery("DefaultFallback");
        }
        else
        {
            List<Node> nodelist = active.getList();
            Debug.Log("THE MATCHED INTENT IS:" + nodelist[0].getIntent());
            if (isPlaying)
            {
                Debug.Log("tried to play timelines while there are others playing");
                return;
            }
            foreach (Node n in nodelist)
            {
                queuedTimelines.Enqueue(n);
            }
            StartCoroutine(playQueue());
        }
    }
    //Triggered by DialogFlow, param query is the result returned by DialogFlow
    //The query result is then matched to NodeDictionary by intent to retrieve List<Nodes>
    
    public void ReturnQuery(QueryResult query)
    {
        Debug.Log("REACHED HERE");
        intent = query.intent.displayName;
        Debug.Log("THE DF INTENT IS:" + intent);
        NodeList active = MatchIntent(intent);
        if (active == null)//If intent from DF is not matched with keys in dictionary 
        {
            ReturnQuery("DefaultFallback");//async intent matching 
        }
        else
        {
            List<Node> nodelist = active.getList();
            Debug.Log("THE MATCHED INTENT IS:" + nodelist[0].getIntent());
            if (isPlaying)
            {
                Debug.Log("tried to play timelines while there are others playing");
                //return;
            }
            foreach (Node n in nodelist)
            {
                queuedTimelines.Enqueue(n);
            }
            StartCoroutine(playQueue());
           
        }
    }
    
    public IEnumerator playQueue()
             {
              isPlaying = true;
              while (queuedTimelines.Count > 0)
                 {
            Node cur = queuedTimelines.Dequeue();
            cur.Play(this);
            Debug.Log("response is: " + cur.getResponse());
            timelineController.Play(cur);
            TimelineAsset currentTimeline = timelineController.PlayFromTimelines(cur.getTaid());

            yield return new WaitForSeconds((float)currentTimeline.duration);
        }
        isPlaying = false;
    }
    
    //key/value search in NodeDictionary
    public NodeList MatchIntent(string intent)
    {
        NodeList MatchedIntent = nodeDictionary[intent];
        if (!(MatchedIntent == null))
        {
            return nodeDictionary[intent];
        }
        return null; 
    }
 
    //Root node logic, agent triggered by box collider attached to player 
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Debug.Log(collider.gameObject.name);
            StartCoroutine(GetGreeting());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
    } 
    //Root node logic, random prompt generator 
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