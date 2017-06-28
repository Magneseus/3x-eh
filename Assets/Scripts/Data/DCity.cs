using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCity : TurnUpdatable
{    
    private Dictionary<string, DBuilding> buildings = new Dictionary<string, DBuilding>();
    private Dictionary<int, DResource> resources = new Dictionary<int, DResource>();
    private Dictionary<int, DPerson> people = new Dictionary<int, DPerson>();

    private CityController cityController;
    private string cityName;
    private int cityAge;
        
    public DCity(string cityName, CityController cityController)
    {        
        this.cityName = cityName;
        this.cityController = cityController;
        this.cityAge = 0;
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {        
        foreach (var entry in buildings)        
            entry.Value.TurnUpdate(numDaysPassed);

        foreach (var entry in resources)
            entry.Value.TurnUpdate(numDaysPassed);

        cityAge += numDaysPassed;
    }

    public void AddPerson(DBuilding dBuilding, DPerson dPerson)
    {
        if (!people.ContainsKey(dPerson.Id))
            people.Add(dPerson.Id, dPerson);

        dBuilding.AddPersonToBuilding(dPerson);
    }

    public void MovePerson(DPerson person, DBuilding desinationBuilding)
    {
        if (desinationBuilding.City != this)
            throw new BuildingNotInCityException(desinationBuilding.Name);

        // Remove person from current building, if any
        if (person.Building != null)
        {
            var oldBuilding = person.Building;
            oldBuilding.RemovePerson(person);
        }

        // TODO: Catch exceptions like BuildingIsFull, when implemented
        desinationBuilding.AddPersonToBuilding(person);
    }

    public void AddResource(DResource resource)
    {
        if (resources.ContainsKey(resource.Id))
        {
            resources[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resources.Add(resource.Id, DResource.Create(resource, resource.Amount));
        }
    }

    public void ConsumeResource(DResource resource)
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

    public DResource GetResource(string name)
    {
        int resourceId = DResource.NameToId(name);
        if (resources.ContainsKey(resourceId))
        {
            return resources[resourceId];
        }
        else
        {
            AddResource(DResource.Create(name));
            return resources[resourceId];
        }
        
    }

    public Dictionary<string, DBuilding> Buildings
    {
        get { return buildings; }
    }

    public Dictionary<int, DResource> Resources
    {
        get { return resources; }
    }

    public Dictionary<int, DPerson> People
    {
        get { return people; }
    }

    public string Name
    {
        get { return cityName; }
        set { cityName = value; }
    }   

    public int Age
    {
        get { return cityAge; }
    }

    public CityController CityController
    {
        get { return cityController; }        
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

public class BuildingNotInCityException : Exception
{
    public BuildingNotInCityException()
    {
    }

    public BuildingNotInCityException(string message)
    : base(message)
    {
    }

    public BuildingNotInCityException(string message, Exception inner)
    : base(message, inner)
    {
    }
}