using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManager {

    // Maybe we want this to be a reference to a Map?
    public List<City> ListOfCities;
    public int DurationOfTurn = 7;
    private int CurrentTurnNumber;
    private int DaysTranspired;

    // Use this for initialization
    void Start () {  
        CurrentTurnNumber = 0;
        DaysTranspired = 0;
    }

    // Turn Update
    public void EndTurnUpdate()
    {
        // Here we will update everything (basically just updating the cities)
        foreach (City c in ListOfCities)
        {
            c.TurnUpdate(DurationOfTurn);
        }

        CurrentTurnNumber += 1;
        DaysTranspired += DurationOfTurn;
        Debug.Log("Turn ended, " + DurationOfTurn + " more days passed. "+ DaysTranspired+" transpired.");
    }
    
}
