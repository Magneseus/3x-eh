using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBuilding : TurnUpdatable {

    private BuildingController buildingController;

    private DCity city;
    private Dictionary<int, DResource> resourceConsumptionPerTurn = new Dictionary<int, DResource>();
    private Dictionary<int, DResource> resourceOutputPerTurn = new Dictionary<int, DResource>();
    private List<DPerson> listOfPersons = new List<DPerson>();
    private String buildingName;

    public DBuilding(DCity city, string buildingName, BuildingController buildingController)
    {        
        this.city = city;        
        this.buildingName = buildingName;
        this.buildingController = buildingController;

        this.city.Buildings.Add(buildingName, this);
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Output before consuming
        foreach (KeyValuePair<int, DResource> entry in resourceOutputPerTurn)
        {
            // Very basic, increase amount by number of population for all resources
            var outputResource = DResource.Create(entry.Value, entry.Value.Amount);
            outputResource.Amount += listOfPersons.Count;
            city.AddResource(outputResource);
        }

        foreach (KeyValuePair<int, DResource> entry in resourceConsumptionPerTurn)
        {
            city.ConsumeResource(entry.Value);
        }
        
    }

    public void AddPersonToBuilding(DPerson person)
    {
        // TODO: Check for building population cap or other limiting factors

        if (listOfPersons.Contains(person))
            return;

        listOfPersons.Add(person);
        person.Building = this;
        City.AddPerson(this, person);
    }

    public void RemovePerson(DPerson person)
    {
        if (!listOfPersons.Contains(person))
            throw new PersonNotFoundException("Person in building: " + (person.Building == null ? "null" : person.Building.Name));

        listOfPersons.Remove(person);
        person.Building = null;
    }

    public void AddResourceOutput(DResource resource)
    {        
        if (resourceOutputPerTurn.ContainsKey(resource.Id))
        {
            resourceOutputPerTurn[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resourceOutputPerTurn.Add(resource.Id, DResource.Create(resource, resource.Amount));
        }        
    }

    public void AddResourceConsumption(DResource resource)
    {
        if (resourceConsumptionPerTurn.ContainsKey(resource.Id))
        {
            resourceConsumptionPerTurn[resource.Id].Amount += resource.Amount;
        }
        else
        {
            resourceConsumptionPerTurn.Add(resource.Id, DResource.Create(resource, resource.Amount));
        }
    }

    public Dictionary<int, DResource> ResourceConsumption
    {
        get { return resourceConsumptionPerTurn; }
    }

    public Dictionary<int, DResource> ResourceOutput
    {
        get { return resourceOutputPerTurn; }
    }

    public DCity City
    {
        get { return city; }
    }

    public List<DPerson> Population
    {
        get { return listOfPersons; }
    }

    public String Name
    {
        get { return buildingName; }
        set { buildingName = value; }
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