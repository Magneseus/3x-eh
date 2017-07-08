using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DGame
{   
    
    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();

    int durationOfTurn = 7;
    int currentTurnNumber = 0;
        
    public void EndTurnUpdate()
    {        
        foreach (var kvp in cities)
        {
            kvp.Value.TurnUpdate(durationOfTurn);
        }

        currentTurnNumber += 1;
    }

    public void AddCity(DCity dCity)
    {
        cities.Add(dCity.Name, dCity);
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

    public int DaysTranspired
    {
        get { return TurnDuration * TurnNumber; }
    }
}
