using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TurnUpdatable {

    private City city;
    private Dictionary<int, Resource> resourceConsumptionPerTurn = new Dictionary<int, Resource>();
    private Dictionary<int, Resource> resourceOutputPerTurn = new Dictionary<int, Resource>();
    private List<Person> listOfPersons = new List<Person>();    
    private int civilianCapacity = 0;

    public Building(City city)
    {
        this.city = city;
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        
    }

    public void AddResourceOutput(Resource resource)
    {        
        if (resourceOutputPerTurn.ContainsKey(resource.Id))
        {
            resourceOutputPerTurn[resource.Id].Amount += resource.Amount;
        } else
        {
            resourceOutputPerTurn.Add(resource.Id, Resource.Create(resource));
        }        
    }

    public void AddResourceConsumpTion(Resource resource)
    {
        if (resourceConsumptionPerTurn.ContainsKey(resource.Id))
        {
            resourceConsumptionPerTurn[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resourceConsumptionPerTurn.Add(resource.Id, Resource.Create(resource));
        }
    }

    public Dictionary<int, Resource> ResourceConsumption
    {
        get { return resourceConsumptionPerTurn; }
    }

    public Dictionary<int, Resource> ResourceOutput
    {
        get { return resourceOutputPerTurn; }
    }

    public City City
    {
        get { return city; }
    }

    public int PopulationCapacity
    {
        get { return civilianCapacity; }
        set { civilianCapacity = value; }
    }
}
