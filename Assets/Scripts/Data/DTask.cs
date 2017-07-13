using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DTask : TurnUpdatable
{

    private static int NEXT_ID = 0;

    protected int id;
    protected string taskName;
    protected int maxPeople;
    protected DBuilding building;
    protected List<DPerson> listOfPeople;
    protected DResource output;
    protected bool taskEnabled;

    private float structuralDamage;
    private float fungalDamage;


    public DTask(DBuilding dBuilding, DResource dOutput, int dMaxPeople, string dName)
    {
        listOfPeople = new List<DPerson>();

        id = NEXT_ID++;
        taskName = dName;
        building = dBuilding;
        output = dOutput;
        maxPeople = dMaxPeople;

        structuralDamage = Constants.TASK_MIN_STRUCTURAL_DMG;
        fungalDamage = Constants.TASK_MIN_FUNGAL_DMG;

        taskEnabled = true;
        dBuilding.AddTask(this);
    }

    public DTask(DBuilding dBuilding, DResource dOutput) : this(dBuilding, dOutput, 4, "default_task")
    {
    }

    public virtual void TurnUpdate(int numDaysPassed)
    {
        if (listOfPeople.Count > 0)
        {
            // TODO: Make this into a exponential scale or something
            if(structuralDamage >= 1.0f) EnableTask();
            for (int i = 0; i < listOfPeople.Count; ++i)
                building.OutputResource(output);
        }
    }

    public void AddPerson(DPerson dPerson)
    {
        if (listOfPeople.Count >= maxPeople)
        {
            throw new TaskFullException(taskName);
        }
        else if (listOfPeople.Contains(dPerson))
        {
            throw new PersonAlreadyAddedException(taskName);
        }
        else
        {
            listOfPeople.Add(dPerson);

            if (dPerson.Task != this)
            {
                dPerson.SetTask(this);
            }
        }
    }

    public void RemovePerson(DPerson dPerson)
    {
        if (listOfPeople.Contains(dPerson))
        {
            listOfPeople.Remove(dPerson);
            dPerson.RemoveTask(this);
        }
        else
        {
            throw new PersonNotFoundException(taskName);
        }
    }

    public void EnableTask()
    {
        taskEnabled = true;
    }

    public void DisableTask()
    {
        // Remove people from task
        while (ListOfPeople.Count > 0)
        {
            ListOfPeople[0].ClearTask();
        }

        // Disable task
        taskEnabled = false;
    }
    public void Repair(float amount)
    {
        if(Infected)
        {
            fungalDamage -= amount;
            fungalDamage = Mathf.Clamp(fungalDamage, Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG);
        }
        else if(Damaged)
        {
            structuralDamage -= amount;
            structuralDamage = Mathf.Clamp(structuralDamage, Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG);
        }
    }

    #region Properties
    public float LevelDamaged
    {
        get { return structuralDamage; }
        set { structuralDamage = Mathf.Clamp(value, Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG); }
    }

    public float LevelInfected
    {
        get { return fungalDamage; }
        set { fungalDamage = Mathf.Clamp(value, Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG); }
    }

    public bool Damaged
    {
        get { return structuralDamage != Constants.TASK_MIN_STRUCTURAL_DMG; }
    }

    public bool Infected
    {
        get { return fungalDamage != Constants.TASK_MIN_FUNGAL_DMG; }
    }

    public int MaxPeople
    {
        get { return maxPeople; }
        set { maxPeople = value; }
    }

    public List<DPerson> ListOfPeople
    {
        get { return listOfPeople; }
    }

    public string Name
    {
        get { return taskName; }
        set { taskName = value; }
    }

    public int ID
    {
        get { return id; }
    }

    public DResource Output
    {
        get { return output; }
    }

    public DBuilding Building
    {
        get { return building; }
    }

    public bool Enabled
    {
        get { return taskEnabled; }
    }
    #endregion
}


#region Exceptions

public class TaskFullException : Exception
{
    public TaskFullException()
    {
    }

    public TaskFullException(string message) : base(message)
    {
    }

    public TaskFullException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TaskFullException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
