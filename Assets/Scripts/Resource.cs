using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : TurnUpdatable
{
    private static int resourceIdCounter = 0;
    private int resourceId;
    private string resourceName = "NameUndefined";
    private int resourceAmount = 0;


    public Resource()
    {
        resourceId = resourceIdCounter++;        
    }

    public Resource(string name, int amount)
    {
        resourceId = resourceIdCounter++;
        resourceName = name;
        resourceAmount = amount;
    }
    
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }

    public int Id
    {
        get { return resourceId; }
    }

    public string Name
    {
        get { return resourceName; }
    }

    public int Amount
    {
        get { return resourceAmount; }
        set { resourceAmount = value; }
    }

}
