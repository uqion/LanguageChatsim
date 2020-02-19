using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexiled.SoHi;

[CreateAssetMenu]
public class TransactionNode : Node
{
    SO_Database database = CreateInstance("SO_Database") as SO_Database;
    ShoppingCart shoppingCart = new ShoppingCart();


    public IEnumerator GetBag(TimelineController timeline)
    {

        timeline.Play(14);
        timeline.Play(15);
        yield return null;
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
}

