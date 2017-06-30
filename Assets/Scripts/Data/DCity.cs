using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class DCity : TurnUpdatable
{
    private CityController cityController;

    private Dictionary<int, DBuilding> buildings = new Dictionary<int, DBuilding>();
    private Dictionary<int, DResource> resources = new Dictionary<int, DResource>();
    private Dictionary<int, DPerson> people = new Dictionary<int, DPerson>();
    
    private int age;
    private string name;
        
    public DCity(string cityName, CityController cityController)
    {        
        name = cityName;
        this.cityController = cityController;
        age = 0;
    }

    public void AddBuilding(DBuilding dBuilding)
    {
        if (buildings.ContainsKey(dBuilding.Id))
        {
            throw new BuildingAlreadyAddedException(string.Format("City '{0}' already has building '{1}'", name, dBuilding.Name));
        }
        else
        {
            buildings.Add(dBuilding.Id, dBuilding);
        }
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {        
        foreach (var entry in buildings)        
            entry.Value.TurnUpdate(numDaysPassed);

        foreach (var entry in resources)
            entry.Value.TurnUpdate(numDaysPassed);

        foreach (var entry in people)
            entry.Value.TurnUpdate(numDaysPassed);        

        age += numDaysPassed;
    }

    public void AddPerson(DPerson dPerson)
    {
        if (people.ContainsKey(dPerson.Id))
        {
            throw new PersonAlreadyAddedException(string.Format("Person already added to city"));
        }
            people.Add(dPerson.Id, dPerson);                
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

    public Dictionary<int, DBuilding> Buildings
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
        get { return name; }
        set { name = value; }
    }   

    public int Age
    {
        get { return age; }
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

public class BuildingNotFoundException : Exception
{
    public BuildingNotFoundException()
    {
    }

    public BuildingNotFoundException(string message)
    : base(message)
    {
    }

    public BuildingNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

public class BuildingAlreadyAddedException : Exception
{
    public BuildingAlreadyAddedException()
    {
    }

    public BuildingAlreadyAddedException(string message) : base(message)
    {
    }

    public BuildingAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BuildingAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

public class PersonNotFoundException : Exception
{
    public PersonNotFoundException()
    {
    }

    public PersonNotFoundException(string message)
    : base(message)
    {
    }

    public PersonNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

public class PersonAlreadyAddedException : Exception
{
    public PersonAlreadyAddedException()
    {
    }

    public PersonAlreadyAddedException(string message) : base(message)
    {
    }

    public PersonAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PersonAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}