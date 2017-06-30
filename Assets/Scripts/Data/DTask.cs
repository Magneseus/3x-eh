using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTask : TurnUpdatable
{

    private static int NEXT_ID = 0;

    private int id;
    private DBuilding building;
    private DPerson person;
    private DResource output;

    public DTask(DBuilding dBuilding, DResource dOutput)
    {
        id = NEXT_ID++;
        building = dBuilding;
        output = dOutput;

        dBuilding.AddTask(this);
    }

    public void ClearPerson()
    {
        person = null;
    }

    public DPerson Person
    {
        get { return person; }
        set { person = value; }
    }

    public int Id
    {
        get { return id; }
    }

    public DResource Output
    {
        get { return output; }
    }

    public void TurnUpdate(int numDaysPassed)
    {
        if (person != null)
        {
            building.OutputResource(output);
        }
    }
}
