using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTask : TurnUpdatable
{

    private static int NEXT_ID = 0;

    private int id;
    private string taskName;
    private DBuilding building;
    private DPerson person;
    private DResource output;

    public DTask(DBuilding dBuilding, DResource dOutput, string name)
    {
        id = NEXT_ID++;
        building = dBuilding;
        output = dOutput;

        taskName = "default_task";

        dBuilding.AddTask(this);
    }

    public DTask(DBuilding dBuilding, DResource dOutput) : this(dBuilding, dOutput, "default_name")
    {
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

    public string Name
    {
        get { return taskName; }
        set { taskName = value; }
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
