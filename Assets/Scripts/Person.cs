using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : TurnUpdatable
{
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



    private Building building;

    public Person()
    {
        this.building = null;
    }

    public Person(Building building)
    {
        this.building = building;
    }
       
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }


    public Building Building
    {
        get { return building; }
        set { building = value; }
    }
}
