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
    protected DBuilding building;
    protected int maxPeople;
    protected List<DPerson> listOfPeople;
    protected DResource output;
    protected bool taskEnabled;
    protected bool reclaimed;
    private float levelReclaimed;


    public DTask(DBuilding dBuilding, DResource dOutput, int dMaxPeople, string dName)
    {
        id = NEXT_ID++;
        building = dBuilding;
        output = dOutput;
        maxPeople = dMaxPeople;
        taskName = dName;

        listOfPeople = new List<DPerson>();
        taskEnabled = true;
        levelReclaimed = 0.1f;

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
            if(levelReclaimed >= 1.0f) EnableTask();
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
    public void IncreaseReclaimed(float amount)
    {
        if (levelReclaimed < 1.0f)
            levelReclaimed = Mathf.Clamp01(levelReclaimed + amount);
    }
    #region Accessors
    public float LevelReclaimed
    {
        get { return levelReclaimed; }
        set { levelReclaimed = Mathf.Clamp01(value); }     // should only be used for dev, increase with Reclaim()
    }
    public bool IsReclaimed()
    {
        return reclaimed;
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
