using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : TurnUpdatable
{
    private static int resourceIdCounter = 0;
    private static Dictionary<string, int> resourceNameToIdMap = new Dictionary<string, int>();

    private int resourceId;
    private string resourceName = "NameUndefined";
    private int resourceAmount = 0;

    public static Resource Create(string name, int amount)
    {
        if (resourceNameToIdMap.ContainsKey(name))
        {
            return new Resource(resourceNameToIdMap[name], name, amount);
        }
        else
        {
            return new Resource(name, amount);
        }
    }

    public static Resource Create(Resource resource)
    {
        return Create(resource.Name, resource.Amount);
    }

    private Resource(int id, string name, int amount)
    {
        resourceId = id;
        resourceName = name;
        resourceAmount = amount;
    }

    private Resource(string name, int amount)
    {

        resourceId = resourceIdCounter++;
        resourceNameToIdMap.Add(name, resourceId);
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
