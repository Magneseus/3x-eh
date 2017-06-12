using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : TurnUpdatable
{

    private Building emptyBuilding;
    private List<Building> listOfBuildings = new List<Building>();
    private Dictionary<int, Resource> resources = new Dictionary<int, Resource>();

    private string cityName = "NoCityName";
    private int cityAge = 0;

    public City()
    {
        emptyBuilding = new Building(this);
        listOfBuildings.Add(emptyBuilding);
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
        foreach (Person p in emptyBuilding.Population)
        {
            p.TurnUpdate(numDaysPassed);
        }

        cityAge += numDaysPassed;
    }

    public void MovePerson(Person person, Building desinationBuilding)
    {
        // Remove person from current building, if any
        if (person.Building != null)
        {
            var oldBuilding = person.Building;
            oldBuilding.RemovePerson(person);
        }

        // TODO: Catch exceptions like BuildingIsFull, when implemented
        desinationBuilding.AddPerson(person);
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

    public Building EmptyBuilding
    {
        get { return emptyBuilding; }
    }

    public List<Person> Population
    {
        get { return emptyBuilding.Population; }
    }

    public string Name
    {
        get { return cityName; }
        set { cityName = value; }
    }

    public int CivilianCount
    {
        get { return emptyBuilding.Population.Count; }
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