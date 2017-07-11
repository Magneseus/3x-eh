using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DGame
{

    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
    private DateTime currentDate = new DateTime(2017,1,1);
    int durationOfTurn = 7;
    int currentTurnNumber = 0;

    public void EndTurnUpdate()
    {
        foreach (var kvp in cities)
        {
            kvp.Value.TurnUpdate(durationOfTurn);
        }

      currentDate =  currentDate.AddDays(durationOfTurn);
        currentTurnNumber += 1;
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
        get { return currentDate.ToString(); }
    }
    public int DaysTranspired
    {
        get { return TurnDuration * TurnNumber; }
    }
}
