using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleJSON;

[Serializable]
public class DGame
{
    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
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

    // Called when the current city is completed
    public void CompletedCurrentCity()
    {
        CollapseCity(currentCity);

        gameController.ReturnToMap(true);

        currentCity = null;
    }

    //TODO: this function and associated class
    // Collapses the city into a set of passive bonuses for future cities
    public void CollapseCity(DCity city)
    {

    }


    public void EndTurnUpdate()
    {
        currentDate = currentDate.AddDays(durationOfTurn);
        currentTurnNumber += 1;
        
        if (currentCity != null)
        {
            currentCity.TurnUpdate(durationOfTurn);
            currentCity.UpdateSeason(currentDate);
        }

        // If we've finished the current city
        if (currentTurnNumber >= turnDurationOfCity)
        {
            CompletedCurrentCity();
        }
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

    public int CityDuration
    {
        get { return turnDurationOfCity; }
    }

    #endregion

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save the current city
        if (currentCity == null)
            returnNode.Add("currentCity", new JSONNull());
        else
            returnNode.Add("currentCity", currentCity.SaveToJSON());

        // Save the current date
        returnNode.Add("currentDate", new JSONString(currentDate.ToShortDateString()));

        // Save the turn information
        returnNode.Add("turnDurationOfCity", new JSONNumber(turnDurationOfCity));
        returnNode.Add("durationOfTurn", new JSONNumber(durationOfTurn));
        returnNode.Add("currentTurnNumber", new JSONNumber(currentTurnNumber));

        return returnNode;
    }

    public static DGame LoadFromJSON(JSONNode jsonNode, GameController gameController)
    {
        DGame dGame = new DGame(gameController);

        // Load the current city
        if (jsonNode["currentCity"].IsNull)
        {
            dGame.currentCity = null;
        }
        else
        {
            // TODO: Add the city controller
            dGame.currentCity = DCity.LoadFromJSON(jsonNode["currentCity"]);
        }

        // Load the current date
        string[] mmddyyyy = ((string)(jsonNode["currentDate"])).Split('/');
        dGame.currentDate = new DateTime(
            int.Parse(mmddyyyy[0]),
            int.Parse(mmddyyyy[1]),
            int.Parse(mmddyyyy[2]));

        // Load the turn information
        dGame.turnDurationOfCity = jsonNode["turnDurationOfCity"].AsInt;
        dGame.durationOfTurn = jsonNode["durationOfTurn"].AsInt;
        dGame.currentTurnNumber = jsonNode["currentTurnNumber"].AsInt;

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