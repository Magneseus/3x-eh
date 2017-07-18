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
    //private Dictionary<int, DCity> linkedCityKeys = new Dictionary<int, DCity>();
    private List<string> linkedCityKeys = new List<string>();

    private int age;
    private string name;
	private float explorationLevel;
    //map of canada vars
    // private List<string> edges;

    public DBuilding townHall;

    public DCity(string cityName, CityController cityController, List<string> linkedCityKeys = null)
    {
        name = cityName;
        this.cityController = cityController;
        age = 0;
        townHall = null;
		explorationLevel = 0.0f;

        if (linkedCityKeys == null)
        {
            this.linkedCityKeys = new List<string>();
        }
        else
        {
            this.linkedCityKeys = new List<string>();

            foreach (string cityKey in linkedCityKeys)
            {
                this.linkedCityKeys.Add(cityKey);
            }
        }
    }

    public void AddBuilding(DBuilding dBuilding)
    {
        if (buildings.ContainsKey(dBuilding.ID))
        {
            throw new BuildingAlreadyAddedException(string.Format("City '{0}' already has building '{1}'", name, dBuilding.Name));
        }
        else
        {
            buildings.Add(dBuilding.ID, dBuilding);

            if (dBuilding.Name == "Town Hall")
                townHall = dBuilding;
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
        if (people.ContainsKey(dPerson.ID))
        {
            throw new PersonAlreadyAddedException(string.Format("Person already added to city"));
        }
            people.Add(dPerson.ID, dPerson);
    }

    public void AddResource(DResource resource)
    {
        if (resources.ContainsKey(resource.ID))
        {
            resources[resource.ID].Amount += resource.Amount;
        }
        else
        {
            resources.Add(resource.ID, DResource.Create(resource, resource.Amount));
        }
    }

    public void ConsumeResource(DResource resource)
    {
        if (resources.ContainsKey(resource.ID) && resources[resource.ID].Amount >= resource.Amount)
        {
            resources[resource.ID].Amount -= resource.Amount;
        }
        else
        {
            throw new InsufficientResourceException(resource.ID.ToString());
        }
    }


	private float CalculateExploration()
	{
		float countDiscovered = 0.0f;
		foreach(DBuilding dBuilding in buildings.Values) 
		{
			if(dBuilding != townHall)
				if(!(dBuilding.Status == DBuilding.DBuildingStatus.UNDISCOVERED))
					countDiscovered++;
		}
		if(countDiscovered > 0)
			return countDiscovered / (float)(buildings.Count - 1);
		else
			return countDiscovered;
	}

    public DResource GetResource(string name)
    {
        int resourceID = DResource.NameToID(name);
        if (resources.ContainsKey(resourceID))
        {
            return resources[resourceID];
        }
        else
        {
            AddResource(DResource.Create(name));
            return resources[resourceID];
        }

    }
    // map of canada methods
    // public Vector2 MapLocation
    // {
    //     get{ return mapLocation;}
    //     set{  mapLocation = value;}
    // }
    public void setEdges(List<string> s)
    {
      linkedCityKeys = s;
    }

    public void linkToCity(string cityKey)
    {
        if (!linkedCityKeys.Contains(cityKey))
            linkedCityKeys.Add(cityKey);
    }

    public IEnumerable<string> getAllLinkedCityKeys()
    {
        foreach (string key in linkedCityKeys)
            yield return key;
    }

    public bool isLinkedTo(string cityKey)
    {
        return linkedCityKeys.Contains(cityKey);
    }

    public void Explore(float exploreAmount)
    {
        explorationLevel = Mathf.Clamp01(explorationLevel + exploreAmount);
        
        float explorableBuildings = buildings.Count - 1.0f;
        float offsetPercentage = 1.0f / explorableBuildings;
        
        List<DBuilding> UnExploredBuildings = new List<DBuilding>();
        foreach(DBuilding dBuilding in buildings.Values)
        {
            if (dBuilding != townHall)
                if ((dBuilding.Status == DBuilding.DBuildingStatus.UNDISCOVERED))
                {
                    UnExploredBuildings.Add(dBuilding);
                }
        }
        if (explorationLevel - offsetPercentage * (explorableBuildings - UnExploredBuildings.Count) >= offsetPercentage)
        {
            int index = UnityEngine.Random.Range(0, UnExploredBuildings.Count - 1);
            UnExploredBuildings[index].Discover();
        }
        
    }

    #region Properties

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

    public List<string> LinkedCityKeys
    {
        get { return linkedCityKeys; }
        set { linkedCityKeys = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

	public float ExplorationLevel
	{
		get { return explorationLevel;}
	}

    public int Age
    {
        get { return age; }
    }

    public CityController CityController
    {
        get { return cityController; }
    }
    #endregion
}

#region Exceptions
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
#endregion
