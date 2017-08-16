using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleJSON;
using UnityEngine;
using System.IO;

[Serializable]
public class DGame
{
    public enum _gameState { PLAY, EVENT, MENU, NUMELEMENTS};
    public _gameState gameState = _gameState.PLAY;
    public DEvent currentEvent = null;

    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
    Dictionary<string, DCompressedCity> completedCities = new Dictionary<string, DCompressedCity>();
    public List<string> availableCities;

    private DateTime currentDate = new DateTime(2017,4,1);
    private DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };

    public DCity currentCity = null;

    private int turnDurationOfCity = 52;
    private int durationOfTurn = 7;
    private int currentTurnNumber = 0;

    private GameController gameController;

    public DGame(GameController gameController)
    {
        this.gameController = gameController;
        DEvent.dGame = this;
        availableCities = new List<string>();
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
            currentCity.CityController.assignGameController(gameController);
            // Adds completed cities to the selected city
            foreach (string linkedCity in currentCity.LinkedCityKeys)
            {
              foreach( var completed in completedCities)
              {
                if( linkedCity == completed.Key)
                  {
                    currentCity.AddBuilding(completed.Value);
                    completed.Value.assignCity(currentCity);
                    

                  }
              }
            }
            LoadCityEvents();
        }
    }

    public void LoadCityEvents()
    {
        var choiceEvtJson = JSON.Parse(File.ReadAllText(Constants.EVT_CHOICE_EVENTS_PATH));
        for (int i = 0; i < choiceEvtJson.Count; i++)
        {
            DEventSystem.AddEventFromJSON(Constants.EVT_TYPE.CHOICE, currentCity, choiceEvtJson[i]);
        }
        

        var modResourceEvtJson = JSON.Parse(File.ReadAllText(Constants.EVT_MOD_RESOURCE_EVENTS_PATH));
        for (int i = 0; i < modResourceEvtJson.Count; i++)
        {
            DEventSystem.AddEventFromJSON(Constants.EVT_TYPE.MOD_RESOURCE, currentCity, modResourceEvtJson[i]);
        }
    }

    // Called when the current city is completed
    public void CompletedCurrentCity()
    {
        availableCities.Remove(currentCity.Name);
        CollapseCity(currentCity);

        gameController.ReturnToMap(true);

        currentCity = null;
    }

    //TODO: this function and associated class
    // Collapses the city into a set of passive bonuses for future cities
    public void CollapseCity(DCity city)
    {
        completedCities.Add(city.Name, new DCompressedCity(city));
    }

    bool temp = true;
    public void EndTurnUpdate()
    {
        currentDate = currentDate.AddDays(durationOfTurn);
        currentTurnNumber += 1;

        if (currentCity != null)
        {
            currentCity.TurnUpdate(durationOfTurn);
            currentCity.UpdateSeason(currentDate);
        }
        DEventSystem.TurnUpdate();
        NextEvent();

        // If we've finished the current city
        if (currentTurnNumber >= turnDurationOfCity)
        {
            CompletedCurrentCity();
        }
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

    public void Reset()
    {
        cities = new Dictionary<string, DCity>();
        completedCities = new Dictionary<string, DCompressedCity>();

        currentDate = new DateTime(2017, 4, 1);

        currentCity = null;
        currentTurnNumber = 0;
        gameState = DGame._gameState.PLAY;
}

    #region Properties

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
        //set { durationOfTurn = value; }
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

    public int CityDuration
    {
        get { return turnDurationOfCity; }
    }

    public GameController GameController
    {
        get { return gameController; }
    }

    #endregion

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save the static resource dictionary
        returnNode.Add("resourceNames", DResource.SaveResourceIDMapToJSON());

        // Save the current city
        if (currentCity == null)
            returnNode.Add("currentCity", new JSONNull());
        else
            returnNode.Add("currentCity", currentCity.SaveToJSON());

        // Save the current date
        JSONNode dateJSON = new JSONObject();
        dateJSON.Add("day", new JSONNumber(currentDate.Day));
        dateJSON.Add("month", new JSONNumber(currentDate.Month));
        dateJSON.Add("year", new JSONNumber(currentDate.Year));
        returnNode.Add("currentDate", dateJSON);

        // Save the turn information
        returnNode.Add("turnDurationOfCity", new JSONNumber(turnDurationOfCity));
        returnNode.Add("durationOfTurn", new JSONNumber(durationOfTurn));
        returnNode.Add("currentTurnNumber", new JSONNumber(currentTurnNumber));

        // Save the list of available cities
        JSONArray availableCityList = new JSONArray();
        foreach (string cityName in availableCities)
        {
            availableCityList.Add(new JSONString(cityName));
        }
        returnNode.Add("availableCities", availableCityList);

        return returnNode;
    }

    public static DGame LoadFromJSON(JSONNode jsonNode, GameController gameController)
    {
        DGame dGame = new DGame(gameController);

        // Load the static resource dictionary
        DResource.LoadResourceIDMapFromJSON(jsonNode["resourceNames"]);

        // Load the current date
        dGame.currentDate = new DateTime(
            RandJSON.JSONInt(jsonNode["currentDate"]["year"]),
            RandJSON.JSONInt(jsonNode["currentDate"]["month"]),
            RandJSON.JSONInt(jsonNode["currentDate"]["day"]));

        // Load the current city
        if (jsonNode["currentCity"].IsNull)
        {
            dGame.currentCity = null;
        }
        else
        {
            dGame.currentCity = DCity.LoadFromJSON(jsonNode["currentCity"], dGame);
        }

        // Load the turn information
        RandJSON.JSONInt(dGame.turnDurationOfCity = jsonNode["turnDurationOfCity"]);
        RandJSON.JSONInt(dGame.durationOfTurn = jsonNode["durationOfTurn"]);
        RandJSON.JSONInt(dGame.currentTurnNumber = jsonNode["currentTurnNumber"]);

        // Load the list of available cities
        foreach (JSONString node in jsonNode["availableCities"].AsArray)
        {
            dGame.availableCities.Add(node.Value);
        }

        return dGame;
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
