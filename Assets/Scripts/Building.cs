using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TurnUpdatable {

    private City city;
    private Dictionary<int, int> resourceConsumptionPerTurn = new Dictionary<int, int>();
    private Dictionary<int, int> resourceOutputPerTurn = new Dictionary<int, int>();
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
            resourceOutputPerTurn[resource.Id] += resource.Amount;
        } else
        {
            resourceOutputPerTurn.Add(resource.Id, resource.Amount);
        }        
    }

    public void AddResourceConsumpTion(Resource resource)
    {
        if (resourceConsumptionPerTurn.ContainsKey(resource.Id))
        {
            resourceConsumptionPerTurn[resource.Id] += resource.Amount;
        }
        else
        {
            resourceConsumptionPerTurn.Add(resource.Id, resource.Amount);
        }
    }

    public Dictionary<int, int> ResourceConsumption
    {
        get { return resourceConsumptionPerTurn; }
    }

    public Dictionary<int, int> ResourceOutput
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
