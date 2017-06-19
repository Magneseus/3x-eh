using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBuilding : TurnUpdatable {

    private City city;
    private Dictionary<int, Resource> resourceConsumptionPerTurn = new Dictionary<int, Resource>();
    private Dictionary<int, Resource> resourceOutputPerTurn = new Dictionary<int, Resource>();
    private List<Person> listOfPersons = new List<Person>();
    private String name;

    public Building(City city)
    {
        this.city = city;
        name = "";
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

    public void AddPerson(Person person)
    {
        // TODO: Check for building population cap or other limiting factors

        if (listOfPersons.Contains(person))
            return;

        listOfPersons.Add(person);
        person.Building = this;
    }

    public void RemovePerson(Person person)
    {
        if (!listOfPersons.Contains(person))
            throw new PersonNotFoundException("Person in building: " + (person.Building == null ? "null" : person.Building.Name));

        listOfPersons.Remove(person);
        person.Building = null;
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

    public String Name
    {
        get { return name; }
        set { name = value; }
    }
}


/*****************************
 *
 *         Exceptions
 *
 *****************************/
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
