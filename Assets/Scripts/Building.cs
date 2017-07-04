using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TurnUpdatable {

    private City city;
    private Dictionary<int, Resource> resourceConsumptionPerTurn = new Dictionary<int, Resource>();
    private Dictionary<int, Resource> resourceOutputPerTurn = new Dictionary<int, Resource>();
    private List<Person> listOfPersons = new List<Person>();
    private String name;
    public enum BuildingStatus { UNDISCOVERED, DISCOVERED, ASSESSED, RECLAIMED };
    private BuildingStatus status;
    private float levelAssessed;
    private float levelReclaimed;
    private int numAssessmentLevels = 5;
    private int numReclaimLevels = 5;
    private float maxAssessmentLevel = 1f;
    private float maxReclaimLevel = 1f;
    private float assessmentLevelInterval;
    private float reclaimLevelInterval;

    public Building(City city)
    {
        this.city = city;
        name = "";
        status = BuildingStatus.UNDISCOVERED;
        levelAssessed = 0f;
        levelReclaimed = 0f;
        assessmentLevelInterval = maxAssessmentLevel / numAssessmentLevels;
        reclaimLevelInterval = maxReclaimLevel / numReclaimLevels;
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

    public void Discover()
    {
        if (IsUndiscovered())
            status = BuildingStatus.DISCOVERED;
    }

    public void Assess()
    {
        if (IsOnlyDiscovered())
            status = BuildingStatus.ASSESSED;
        if (IsDiscovered())
            IncreaseAssessment();
    }

    private void IncreaseAssessment()
    {
        if (levelAssessed < maxAssessmentLevel)
            levelAssessed += assessmentLevelInterval;
    }

    public void Reclaim()
    {
        if (IsOnlyAssessed())
            status = BuildingStatus.RECLAIMED;
        if (IsAssessed())
            IncreaseReclaimed();
    }

    private void IncreaseReclaimed()
    {
        if (levelReclaimed < maxReclaimLevel)
            levelReclaimed += reclaimLevelInterval;
    }

    public float LevelAssessed
    {
        get { return levelAssessed; }
        set { levelAssessed = value; }      // should only be used for dev, increase with Assess()
    }

    public float LevelReclaimed
    {
        get { return levelReclaimed; }
        set { levelReclaimed = value; }     // should only be used for dev, increase with Reclaim()
    }

    public int NumAssessmentLevels
    {
        get { return numAssessmentLevels; }
    }

    public int NumReclaimLevels
    {
        get { return numReclaimLevels; }
    }

    public float AssessmentLevelInterval
    {
        get { return assessmentLevelInterval; }
    }

    public float ReclaimLevelInterval
    {
        get { return reclaimLevelInterval; }
    }

    public float MaxAssessmentLevel
    {
        get { return maxAssessmentLevel; }
    }

    public float MaxReclaimLevel
    {
        get { return maxReclaimLevel; }
    }

    public BuildingStatus Status
    {
        get { return status; }
        set { status = value; }
    }

    public bool IsUndiscovered()
    {
        if (status == BuildingStatus.UNDISCOVERED)
            return true;
        return false;
    }

    public bool IsDiscovered()
    {
        if (!IsUndiscovered())
            return true;
        return false;
    }

    public bool IsOnlyDiscovered()
    {
        if (status == BuildingStatus.DISCOVERED)
            return true;
        return false;
    }

    public bool IsAssessed()
    {
        if (IsOnlyAssessed() || IsReclaimed())
            return true;
        return false;
    }

    public bool IsOnlyAssessed()
    {
        if (status == BuildingStatus.ASSESSED)
            return true;
        return false;
    }

    public bool IsReclaimed()
    {
        if (status == BuildingStatus.RECLAIMED)
            return true;
        return false;
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