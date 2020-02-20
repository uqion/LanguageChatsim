using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    private double runningTotal;

    public ShoppingCart()
    {
        runningTotal = 0.0;
    }

    public void AddItem(double price)
    {
        runningTotal += price;
    }

    public void ResetCart()
    {
        runningTotal = 0;
    }

    public double GetTotal()
    {
        return runningTotal;
    }
}
