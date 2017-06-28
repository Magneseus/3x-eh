using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DGame {


    Dictionary<string, DCity> cities = new Dictionary<string, DCity>();
    int durationOfTurn = 7;
    int currentTurnNumber = 0;

    // Turn Update
    public void EndTurnUpdate()
    {
        // Here we will update everything (basically just updating the cities)
        foreach (var kvp in cities)
        {
            kvp.Value.TurnUpdate(durationOfTurn);
        }

        currentTurnNumber += 1;
       // Debug.Log("Turn ended, " + durationOfTurn + " more days passed. "+ DaysTranspired+" transpired.");
    }

    public void MovePerson(DPerson person, DBuilding destinationBuilding)
    {
        if (destinationBuilding.City == null)
            throw new BuildingNotInCityException("City reference is null in building: " + destinationBuilding.Name);

        destinationBuilding.City.MovePerson(person, destinationBuilding);
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
