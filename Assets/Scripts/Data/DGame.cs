using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class DGame
{
    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
    private DateTime currentDate = new DateTime(2017,4,1);
    private DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };

    public DCity currentCity = null;
    int durationOfTurn = 7;
    int currentTurnNumber = 0;

    public DGame()
    {
        
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

            if (temp)
            {
                temp = false;
                DEventSystem.AddEvent(new ModifyResourceEvent(kvp.Value, DResource.Create("Food", 50)));
            }
        }
        DEventSystem.TurnUpdate();
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