using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DResource : TurnUpdatable
{
    private static int resourceIdCounter = 0;
    private static Dictionary<string, int> resourceNameToIdMap = new Dictionary<string, int>();

    private int resourceId;
    private string resourceName = "NameUndefined";
    private int resourceAmount = 0;
        
    // Primary Factory
    public static DResource Create(string name, int amount)
    {
        if (resourceNameToIdMap.ContainsKey(name))
        {
            return new DResource(resourceNameToIdMap[name], name, amount);
        }
        else
        {
            return new DResource(name, amount);
        }
    }

    public static DResource Create(string name)
    {
        return Create(name, 0);
    }

    public static DResource Create(DResource resource)
    {
        return Create(resource.Name);
    }

    public static DResource Create(DResource resource, int amount)
    {
        return Create(resource.Name, amount);
    }

    private DResource(int id, string name, int amount)
    {
        resourceId = id;
        resourceName = name;
        resourceAmount = amount;
    }

    private DResource(string name, int amount)
    {

        resourceId = resourceIdCounter++;
        resourceNameToIdMap.Add(name, resourceId);
        resourceName = name;
        resourceAmount = amount;
    }

    public static int NameToId(string name)
    {
        if (resourceNameToIdMap.ContainsKey(name))
        {
            return resourceNameToIdMap[name];
        }
        else
        {
            throw new ResourceNameNotFoundException("Resource name does not exist: " + name);
        }
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
        set { resourceAmount = Math.Max(0, value); }
    }
}

/*****************************
 * 
 *         Exceptions
 *         
 *****************************/
public class ResourceNameNotFoundException : Exception
{
    public ResourceNameNotFoundException()
    {
    }

    public ResourceNameNotFoundException(string message)
    : base(message)
    {
    }

    public ResourceNameNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}