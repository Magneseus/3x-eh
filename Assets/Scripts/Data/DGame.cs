using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DGame {

    // Maybe we want this to be a reference to a Map?
    List<DCity> listOfCities = new List<DCity>();
    int durationOfTurn = 7;
    int currentTurnNumber = 0;

    // Turn Update
    public void EndTurnUpdate()
    {
        // Here we will update everything (basically just updating the cities)
        foreach (DCity c in listOfCities)
        {
            c.TurnUpdate(durationOfTurn);
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

    public List<DCity> Cities
    {
        get { return listOfCities; }
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
