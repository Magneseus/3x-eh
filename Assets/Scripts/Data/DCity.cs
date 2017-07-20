﻿using System;
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
    private List<string> linkedCityKeys = new List<string>();

    private int age;
    private string name;
    private DSeasons._season season;
    private DateTime[] seasonStartDates = new DateTime[4];
    private DateTime[] deadOfWinterStartEnd = new DateTime[2];
    private bool isDeadOfWinter = false;

    public static float health = 0.5f;
    public static int foodConsumption = 1;
    public static float notEnoughFoodHealthDecay = 0.8f;

    #region Constructors and Init
    public DCity(string cityName, CityController cityController)
    {
        Init(cityName, cityController, DSeasons.DefaultStartDates(), new DateTime(2017,1,1), DSeasons.DefaultDeadOfWinterDates(), null);
    }

    public DCity(string cityName, CityController cityController, DateTime currentDate)
    {
        Init(cityName, cityController, DSeasons.DefaultStartDates(), currentDate, DSeasons.DefaultDeadOfWinterDates(), null);
    }

    public DCity(string cityName, CityController cityController, DateTime[] seasonDates, DateTime currentDate, List<string> linkedCityKeys = null)
    {
        Init(cityName, cityController, seasonDates, currentDate, DSeasons.DefaultDeadOfWinterDates(), linkedCityKeys);
    }

    private void Init(string cityName, CityController cityController, DateTime[] seasonDates, DateTime currentDate, 
         DateTime[] deadWinterDates, List<string> linkedCityKeys)
    {
        name = cityName;
        this.cityController = cityController;
        age = 0;

        InitialLinkedCities(linkedCityKeys);
        seasonStartDates = DSeasons.InitialSeasonSetup(seasonDates, currentDate, ref season, ref deadWinterDates);
        
    }

    private void InitialLinkedCities(List<string> linkedCityKeys)
    {
        this.linkedCityKeys = new List<string>();
        if (linkedCityKeys != null)
            foreach (string cityKey in linkedCityKeys)
                this.linkedCityKeys.Add(cityKey);
    }

    private void InitialDeadOfWinter(DateTime currentDate, DateTime[] deadWinterDates)
    {
        deadOfWinterStartEnd = deadWinterDates;
        if (currentDate < deadOfWinterStartEnd[1] && currentDate >= deadOfWinterStartEnd[0])
            isDeadOfWinter = true;
    }
#endregion

    #region Update Calls
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        UpdateBuildings(numDaysPassed);
        UpdateResources(numDaysPassed);
        UpdatePeople(numDaysPassed);
        UpdateCity();

        age += numDaysPassed;
    }

    public void UpdateSeason(DateTime currentDate)
    {
        seasonStartDates = DSeasons.UpdateSeasonStatus(seasonStartDates, currentDate, ref season);
    }

    public void UpdateDeadOfWinter(DateTime currentDate)
    {
        if (season == DSeasons._season.WINTER &&
            currentDate < deadOfWinterStartEnd[1] && currentDate >= deadOfWinterStartEnd[0])
            isDeadOfWinter = true;
    }

    #region Update Other Elements
    private void UpdateBuildings(int numDaysPassed)
    {
        foreach (var entry in buildings)
        {
            entry.Value.TurnUpdate(numDaysPassed);
            BuildingSeasonalEffects(entry.Value, numDaysPassed);
        }
    }

    private void BuildingSeasonalEffects(DBuilding building, int numDaysPassed)
    {
        switch (season)
        {
            case DSeasons._season.SPRING:
                building.SpringEffects();
                break;
            case DSeasons._season.SUMMER:
                building.SummerEffects();
                break;
            case DSeasons._season.FALL:
                building.FallEffects();
                break;
            case DSeasons._season.WINTER:
                building.WinterEffects();
                break;
        }
    }

    private void UpdateResources(int numDaysPassed)
    {
        foreach (var entry in resources)
            entry.Value.TurnUpdate(numDaysPassed);
    }

    private void UpdatePeople(int numDaysPassed)
    {
        int exploringInWinter = 0;
        foreach (var entry in people)
        {
            entry.Value.TurnUpdate(numDaysPassed);
            UpdatePeopleWinter(entry, ref exploringInWinter);
        }
        if (exploringInWinter > 1)
            health = Mathf.Clamp(health - DSeasons.reduceHealthExploringWinter, 0f, 1f);
    }

    private void UpdatePeopleWinter(KeyValuePair<int, DPerson> entry, ref int exploringInWinter)
    {
        if (entry.Value.Task.GetType() == typeof(DTask_Explore) && season == DSeasons._season.WINTER)
        {
            exploringInWinter++;
            DeadOfWinterCulling(entry);
        }
    }

    public void DeadOfWinterCulling(KeyValuePair<int,DPerson> entry)
    {
        if (isDeadOfWinter)
        {
            entry.Value.Dies();
            people.Remove(entry.Key);
        }
    }
    #endregion

    public void UpdateCity()
    {
        UpdateCityFood();
        UpdateCityHealth();
    }

    public void UpdateCityFood()
    {
        DResource resource = GetResource(Constants.FOOD_RESOURCE_NAME);
        int consumeAmount = (int)(foodConsumption * DSeasons.modFoodConsumption[(int)season]);
        if (resource.Amount >= foodConsumption)
            ConsumeResource(resource, foodConsumption);
        else
        {
            ConsumeResource(resource);
            NotEnoughFood(foodConsumption - resource.Amount);
        }
    }

    public void NotEnoughFood(int deficit)
    {
        health *= notEnoughFoodHealthDecay;
    }

    public void UpdateCityHealth()
    {

    }
    #endregion

    #region Basic Manipulation
    public void AddBuilding(DBuilding dBuilding)
    {
        if (buildings.ContainsKey(dBuilding.ID))
        {
            throw new BuildingAlreadyAddedException(string.Format("City '{0}' already has building '{1}'", name, dBuilding.Name));
        }
        else
        {
            buildings.Add(dBuilding.ID, dBuilding);
        }
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
        int amount = (int)(resource.Amount * SeasonResourceMod(resource));
        if (resources.ContainsKey(resource.ID))
        {
            resources[resource.ID].Amount += amount;
        }
        else
        {
            resources.Add(resource.ID, DResource.Create(resource, amount));
        }
    }

    // todo - as resources are defined with constant names, include if checks here
    public float SeasonResourceMod(DResource resource)
    {
        if (resource.Name == Constants.FOOD_RESOURCE_NAME)
            return DSeasons.modFoodProduction[(int)season];
        return 1f;
    }

    public void ConsumeResource(DResource resource, int amount)
    {
        if (resources.ContainsKey(resource.ID) && resources[resource.ID].Amount >= amount)
        {
            resources[resource.ID].Amount -= amount;
        }
        else
        {
            throw new InsufficientResourceException(resource.ID.ToString());
        }
    }

    public void ConsumeResource(DResource resource)
    {
        ConsumeResource(resource, resource.Amount);
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
#endregion

    #region Map of Canada
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
#endregion

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

    public int Age
    {
        get { return age; }
    }

    public CityController CityController
    {
        get { return cityController; }
    }

    public DSeasons._season Season
    {
        get { return season; }
        set { season = value; }
    }

    public DateTime[] SeasonStartDates
    {
        get { return seasonStartDates; }
        set { seasonStartDates = value; }
    }

    public DateTime[] DeadOfWinterDates
    {
        get { return deadOfWinterStartEnd; }
        set { deadOfWinterStartEnd = value; }
    }

    public bool IsDeadOfWinter
    {
        get { return isDeadOfWinter; }
        set { isDeadOfWinter = value; }
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
