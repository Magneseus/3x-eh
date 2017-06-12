using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TurnUpdatable {

    private City city;
    private Dictionary<int, Resource> resourceConsumptionPerTurn = new Dictionary<int, Resource>();
    private Dictionary<int, Resource> resourceOutputPerTurn = new Dictionary<int, Resource>();
    private List<Person> listOfPersons = new List<Person>();

    public Building(City city)
    {
        this.city = city;
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Output before consuming
        foreach (KeyValuePair<int, Resource> entry in resourceOutputPerTurn)
        {
            // Very basic, increase amount by number of population for all resources
            var outputResource = Resource.Create(entry.Value, entry.Value.Amount);
            outputResource.Amount += listOfPersons.Count;
            city.AddResource(outputResource);
        }

        foreach (KeyValuePair<int, Resource> entry in resourceConsumptionPerTurn)
        {
            city.ConsumeResource(entry.Value);
        }
        
    }

    public bool AddPerson(Person person)
    {
        // TODO: Check for building population cap (and return false)

        listOfPersons.Add(person);
        person.Building = this;

        return true;
    }

    public void AddResourceOutput(Resource resource)
    {        
        if (resourceOutputPerTurn.ContainsKey(resource.Id))
        {
            resourceOutputPerTurn[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resourceOutputPerTurn.Add(resource.Id, Resource.Create(resource, resource.Amount));
        }        
    }

    public void AddResourceConsumption(Resource resource)
    {
        if (resourceConsumptionPerTurn.ContainsKey(resource.Id))
        {
            resourceConsumptionPerTurn[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resourceConsumptionPerTurn.Add(resource.Id, Resource.Create(resource, resource.Amount));
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

    public List<Person> Population
    {
        get { return listOfPersons; }
    }
}
