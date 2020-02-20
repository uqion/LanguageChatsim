using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SO_Database : ScriptableObject
{//TODO implement inspector interface for easy entry; implement intent as key in dictionary 
    /// <summary>
    /// grocery items and their price that are available to be purchased .
    /// </summary>
    private Dictionary<string, double> itemPriceDatabase = new Dictionary<string, double>()
    {
        {"UserProvidesBeverageRight", 2.99}
    };

    public double GetPrice(string item)
    {
        return itemPriceDatabase[item];
    }
}
