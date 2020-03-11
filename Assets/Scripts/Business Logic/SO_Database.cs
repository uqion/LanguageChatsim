using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "SO_Database")]
//Using a scriptable object as a database to store prices of each item; interfaces with Transaction Node
public class SO_Database : ScriptableObject
{//TODO implement inspector interface for easy entry; implement intent as key in dictionary 
    /// <summary>
    /// grocery items and their price that are available to be purchased .
    /// </summary>
    [SerializeField]
    Dictionary<string, double> itemPriceDatabase = new Dictionary<string, double>()
    {
        {"UserProvidesBeverageRight", 2.99},
        {"UserCorrectionWB", 2.99 },
        {"Coffee", 3.50 },
        {"HotChocolate", 3.50 }
     };

    public double GetPrice(string item)
    {
        return itemPriceDatabase[item];
    }
}
