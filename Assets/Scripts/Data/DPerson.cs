using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPerson : TurnUpdatable
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



    private DBuilding building;

    public DPerson()
    {
        this.building = null;
    }

    public DPerson(DBuilding building)
    {
        this.building = building;
        building.AddPerson(this);
    }
       
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }


    public DBuilding Building
    {
        get { return building; }
        set { building = value; }
    }
}
