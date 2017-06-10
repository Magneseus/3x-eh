using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : TurnUpdatable
{

    private List<Building> listOfBuildings = new List<Building>();
    private Dictionary<int, Resource> resources = new Dictionary<int, Resource>();    
    private List<Person> listOfPopulation = new List<Person>();

    private string cityName = "NoCityName";
    private int cityAge = 0;

    public City()
    {

    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Here we're probably just going to call TurnUpdate on all the
        // buildings/people/resources
        foreach (Building b in listOfBuildings)
        {
            b.TurnUpdate(numDaysPassed);
        }
        foreach (KeyValuePair<int, Resource> entry in resources)
        {
            entry.Value.TurnUpdate(numDaysPassed);
        }
        foreach (Person p in listOfPopulation)
        {
            p.TurnUpdate(numDaysPassed);
        }

        cityAge += numDaysPassed;
    }

    public void AddResource(Resource resource)
    {
        if (resources.ContainsKey(resource.Id))
        {
            resources[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resources.Add(resource.Id, Resource.Create(resource, resource.Amount));
        }
    }

    public void ConsumeResource(Resource resource)
    {
        if (resources.ContainsKey(resource.Id) && resources[resource.Id].Amount >= resource.Amount)
        {
            resources[resource.Id].Amount -= resource.Amount;
        }
        else
        {
            throw new InsufficientResourceException(resource.Id.ToString());
        }
    }

    public Resource GetResource(string name)
    {
        int resourceId = Resource.NameToId(name);
        if (resources.ContainsKey(resourceId))
        {
            return resources[resourceId];
        }
        else
        {
            AddResource(Resource.Create(name));
            return resources[resourceId];
        }
        
    }

    public Dictionary<int, Resource> Resources
    {
        get { return resources; }
    }

    public List<Building> Buildings
    {
        get { return listOfBuildings; }
    }    

    public List<Person> Population
    {
        get { return listOfPopulation; }
    }

    public string Name
    {
        get { return cityName; }
        set { cityName = value; }
    }

    public int CivilianCount
    {
        get { return listOfPopulation.Count; }
    }

    public int Age
    {
        get { return cityAge; }
    }

}


/*****************************
 * 
 *         Exceptions
 *         
 *****************************/
public class InsufficientResourceException : Exception
{
    public InsufficientResourceException()
    {
    }

    public InsufficientResourceException(string message)
    : base(message)
    {
    }

    public InsufficientResourceException(string message, Exception inner)
    : base(message, inner)
    {
    }
}