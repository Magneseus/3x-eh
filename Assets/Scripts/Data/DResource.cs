using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class DResource : TurnUpdatable
{
    private static int resourceIDCounter = 0;
    private static Dictionary<string, int> resourceNameToIDMap = new Dictionary<string, int>();

    private int resourceID;
    private string resourceName = "NameUndefined";
    private int resourceAmount = 0;
        
    // Primary Factory
    public static DResource Create(string name, int amount)
    {
        if (resourceNameToIDMap.ContainsKey(name))
        {
            return new DResource(resourceNameToIDMap[name], name, amount);
        }
        else
        {
            return new DResource(name, amount);
        }
    }

    public static DResource Create(string name)
    {
        return Create(name, Constants.DEFAULT_RESOURCE_VALUE);
    }

    public static DResource Create(DResource resource)
    {
        return Create(resource.Name);
    }

    public static DResource Create(DResource resource, int amount)
    {
        return Create(resource.Name, amount);
    }

    private DResource(int ID, string name, int amount)
    {
        resourceID = ID;
        resourceName = name;
        resourceAmount = amount;
    }

    private DResource(string name, int amount)
    {

        resourceID = resourceIDCounter++;
        resourceNameToIDMap.Add(name, resourceID);
        resourceName = name;
        resourceAmount = amount;
    }

    public static int NameToID(string name)
    {
        if (resourceNameToIDMap.ContainsKey(name))
        {
            return resourceNameToIDMap[name];
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

    #region Properties

    public int ID
    {
        get { return resourceID; }
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

    #endregion

    public static JSONNode SaveResourceIDMapToJSON()
    {
        JSONNode returnNode = new JSONArray();

        // Save all the currently created resources (the master list)
        foreach (var kvp in resourceNameToIDMap)
        {
            JSONNode resourceEntry = new JSONObject();
            resourceEntry.Add("name", new JSONString(kvp.Key));
            resourceEntry.Add("ID", new JSONNumber(kvp.Value.ToString()));
        }

        return returnNode;
    }

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        returnNode.Add("name", new JSONString(resourceName));
        returnNode.Add("ID", new JSONNumber(resourceID));
        returnNode.Add("amount", new JSONNumber(resourceAmount));

        return returnNode;
    }

    public static void LoadResourceIDMapFromJSON(JSONNode jsonNode)
    {
        Dictionary<string, int> resourceMap = new Dictionary<string, int>();

        // Load all the currently created resources (the master list)
        foreach (JSONNode resource in jsonNode.AsArray)
        {
            resourceMap.Add(resource["name"], resource["ID"]);
        }

        resourceNameToIDMap = resourceMap;
    }

    public static DResource LoadFromJSON(JSONNode jsonNode)
    {
        DResource loadedResource = Create(jsonNode["name"], jsonNode["amount"].AsInt);

        if (loadedResource.ID != jsonNode["ID"].AsInt)
        {
            throw new ResourceDictionaryMismatchException(
                string.Format("Resource ({1}) has saved ID ({2}), but ID should be ({3}).",
                    jsonNode["name"],
                    loadedResource.ID,
                    jsonNode["ID"])
                );
        }

        return loadedResource;
    }
}

#region Exceptions

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

public class ResourceDictionaryMismatchException : Exception
{
    public ResourceDictionaryMismatchException()
    {
    }

    public ResourceDictionaryMismatchException(string message)
    : base(message)
    {
    }

    public ResourceDictionaryMismatchException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

#endregion