using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPerson : TurnUpdatable
{
    private static int NEXT_ID = 0;


    /* TODO: 
     * - rework Person variables
     * - proper access modifiers
     */
    public enum PAge
    {
        Youth,
        Adult,
        Elder
    };

    public PAge  Age;
    public float Happiness;
    public float Hunger;
    // End of TODO

    private DCity dCity;
    private DBuilding dBuilding;
    private int id;

    public DPerson(DBuilding dBuilding)
    {
        id = NEXT_ID++;
        dBuilding.AddPersonToBuilding(this);
    }
       
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }

    public int Id
    {
        get { return id; }
    }

    public DBuilding Building
    {
        get { return dBuilding; }
        set { dBuilding = value; }
    }

    public DCity City
    {
        get { return dCity; }
        set { dCity = value; }
    }
}
