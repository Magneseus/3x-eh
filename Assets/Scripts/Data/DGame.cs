using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class DGame
{
    private static DGame singleton = null;
    public enum _gameState { PLAY, EVENT, MENU, NUMELEMENTS};
    public _gameState gameState = _gameState.PLAY;
    public DEvent currentEvent = null;

    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
    private DateTime currentDate = new DateTime(2017,4,1);
    private DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };

    public DCity currentCity = null;
    int durationOfTurn = 7;
    int currentTurnNumber = 0;

    public static DGame Instance()
    {
        if (singleton == null)
            singleton = new DGame();

        return singleton;
    }

    public DGame()
    {
        if (singleton == null)
            singleton = this;
    }

    // Sets the specified city to be the current "active" city
    public void SelectCity(string cityName)
    {
        if (!cities.ContainsKey(cityName))
        {
            throw new CityNotFoundException(cityName);
        }
        else
        {
            currentCity = cities[cityName];
        }
    }

    //TODO: this function and associated class
    // Collapses the city into a set of passive bonuses for future cities
    public void CollapseCity(string cityName)
    {

    }

    bool temp = true;
    public void EndTurnUpdate()
    {
        currentDate = currentDate.AddDays(durationOfTurn);
        currentTurnNumber += 1;

        foreach (var kvp in cities)
        {
            kvp.Value.TurnUpdate(durationOfTurn);
            kvp.Value.UpdateSeason(currentDate);

            if(temp)
                TEMPtestEvents();
        }
        DEventSystem.TurnUpdate();
        NextEvent();
    }

    public void TEMPtestEvents()
    {
        temp = false;
        int turnsToActivate = 2;

        string prompt2 = "A wolverine is raising her pups in your taco pantry. Do you attempt to remove her?";
        DEvent.activationCondition actCon2 = e => true;
        DResource resource0 = DResource.Create("Food", -25);
        ChoiceEvent.outcome outcome0 = e => e.City.AddResource(resource0);
        ChoiceEvent.outcome outcome1 = e => e.City.Health *= 0.9f;
        ChoiceEvent.outcome[] outcomes = new ChoiceEvent.outcome[] { outcome0, outcome1 };
        string[] outcomeTexts = new string[] { "Her pups will remember the tacos fondly. Your people will not.", "Jared was only slightly maimed removing the wolverine." };
        DEventSystem.AddEvent(new ChoiceEvent(prompt2, currentCity, actCon2, outcomes, outcomeTexts, turnsToActivate));

        string prompt1 = "Oh shit, you found some tacos!\nYou got 50 food.";
        DEvent.activationCondition actCon1 = e => e.City.HasPeopleInTask(typeof(DTask_Explore));
        DEventSystem.AddEvent(new ModifyResourceEvent(prompt1, currentCity, DResource.Create("Food", 50), actCon1, turnsToActivate, Constants.EVENT_PRIORITY_INTERESTING));
    }

    public void NextEvent()
    {
        gameState = _gameState.EVENT;
        DEvent nextEvent = DEventSystem.NextEvent();
        if (nextEvent != null)
            ActivateEvent(nextEvent);
        else
            gameState = _gameState.PLAY;
    }

    public void AddCity(DCity dCity)
    {
        cities.Add(dCity.Name, dCity);
    }

    public void LinkCities(string city1Key, string city2Key)
    {
        cities[city1Key].linkToCity(city2Key);
        cities[city2Key].linkToCity(city1Key);
    }

    public void ActivateEvent(DEvent e)
    {
        if (e.ActivationCondition())
        {
            currentEvent = e;
            currentEvent.Activate();
        }
        else
            NextEvent();
    }

    public void ResolveEvent(int selection = Constants.NO_INPUT)
    {
        currentEvent.Resolve(selection);
        NextEvent();
    }

    public Dictionary<string, DCity> Cities
    {
        get { return cities; }
    }

    public int TurnNumber
    {
        get { return currentTurnNumber; }
        set { currentTurnNumber = value; }
    }

    public int TurnDuration
    {
        get { return durationOfTurn; }
        set { durationOfTurn = value; }
    }
    public string currentDateString
    {
        get { return currentDate.ToShortDateString(); }
    }
    public DateTime CurrentDate
    {
        get { return currentDate; }
    }
    public int DaysTranspired
    {
        get { return TurnDuration * TurnNumber; }
    }
    public DateTime[] DefaultSeasonStartDates
    {
        get { return defaultSeasonStartDates; }
    }

    public _gameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

    public DEvent CurrentEvent
    {
        get { return currentEvent; }
        set { currentEvent = value; }
    }
}

#region Exceptions

public class CityNotFoundException : Exception
{
    public CityNotFoundException()
    {
    }

    public CityNotFoundException(string message) : base(message)
    {
    }

    public CityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion