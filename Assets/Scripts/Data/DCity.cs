using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCity : TurnUpdatable
{

    private DBuilding emptyBuilding;
    private List<DBuilding> listOfBuildings = new List<DBuilding>();
    private Dictionary<int, DResource> resources = new Dictionary<int, DResource>();

    private CityController cityController;
    private string cityName = "NoCityName";
    private int cityAge = 0;
    
    public DCity()
    {        
        emptyBuilding = new DBuilding(this);
        listOfBuildings.Add(emptyBuilding);
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Here we're probably just going to call TurnUpdate on all the
        // buildings/people/resources
        foreach (DBuilding b in listOfBuildings)
        {
            b.TurnUpdate(numDaysPassed);
        }
        foreach (KeyValuePair<int, DResource> entry in resources)
        {
            entry.Value.TurnUpdate(numDaysPassed);
        }
        foreach (DPerson p in emptyBuilding.Population)
        {
            p.TurnUpdate(numDaysPassed);
        }

        cityAge += numDaysPassed;
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
        desinationBuilding.AddPerson(person);
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

    public Dictionary<int, DResource> Resources
    {
        get { return resources; }
    }

    public List<DBuilding> Buildings
    {
        get { return listOfBuildings; }
    }    

    public DBuilding EmptyBuilding
    {
        get { return emptyBuilding; }
    }

    public List<DPerson> Population
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

    public CityController CityController
    {
        get { return cityController; }
        set { cityController = value; }
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