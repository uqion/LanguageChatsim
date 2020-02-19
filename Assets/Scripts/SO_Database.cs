using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_Database : ScriptableObject
{
    /// <summary>
    /// grocery items and their price that are available to be purchased .
    /// </summary>
    private Dictionary<string, double> itemPriceDatabase = new Dictionary<string, double>()
    {
        {"Bottle of Water", 2.99}
    };

    public double GetPrice(string item)
    {
        return itemPriceDatabase[item];
    }
}
