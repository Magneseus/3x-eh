using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DBuilding : TurnUpdatable {

    private static int NEXT_ID = 0;
    private BuildingController buildingController;

    private int id;
    private DCity city;
    private String buildingName;
    private Dictionary<int, DTask> tasks = new Dictionary<int, DTask>();

    public enum BuildingStatus
    {
      UNDISCOVERED,
      DISCOVERED, 
      ASSESSED,
      RECLAIMED
    };
    private BuildingStatus status;
    private float levelAssessed;
    private float levelReclaimed;

    public DBuilding(DCity city, string buildingName, BuildingController buildingController)
    {
        this.id = NEXT_ID++;
        this.city = city;
        this.buildingName = buildingName;
        this.buildingController = buildingController;

        status = BuildingStatus.UNDISCOVERED;
        levelAssessed = 0.1f;
        levelReclaimed = 0.1f;

        city.AddBuilding(this);
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        foreach (var entry in tasks)
        {
            if (entry.Value.Enabled)
                entry.Value.TurnUpdate(numDaysPassed);
        }
    }

    public DTask GetTask(int id)
    {
        if (tasks.ContainsKey(id))
        {
            return tasks[id];
        }
        else
        {
            throw new TaskNotFoundException("Task is not assigned to this building");
        }
    }

    public void AddTask(DTask task)
    {
        if (tasks.ContainsKey(task.ID))
        {
            throw new TaskAlreadyAddedException("Task already assigned to this building");
        }
        else
        {
            tasks.Add(task.ID, task);
        }
    }

    public void OutputResource(DResource resource)
    {
        city.AddResource(resource);
    }

    public DCity City
    {
        get { return city; }
    }

    public Dictionary<int, DTask> Tasks
    {
        get { return tasks; }
    }

    public String Name
    {
        get { return buildingName; }
        set { buildingName = value; }
    }

    public int ID
    {
        get { return id; }
    }

    #region Assessment Components

    public void Discover()
    {
        if (IsUndiscovered())
            status = BuildingStatus.DISCOVERED;
    }

    public void Assess(float amount)
    {
        if (IsOnlyDiscovered())
            status = BuildingStatus.ASSESSED;
        if (IsDiscovered())
            IncreaseAssessment(amount);
    }

    private void IncreaseAssessment(float amount)
    {
        if (levelAssessed < 1.0f)
            levelAssessed = Mathf.Clamp01(levelAssessed + amount);
    }

    public void Reclaim(float amount)
    {
        if (IsOnlyAssessed())
            status = BuildingStatus.RECLAIMED;
        if (IsAssessed())
            IncreaseReclaimed(amount);
    }

    private void IncreaseReclaimed(float amount)
    {
        if (levelReclaimed < 1.0f)
            levelReclaimed = Mathf.Clamp01(levelReclaimed + amount);
    }

    public float LevelAssessed
    {
        get { return levelAssessed; }
        set { levelAssessed = Mathf.Clamp01(value); }      // should only be used for dev, increase with Assess()
    }

    public float LevelReclaimed
    {
        get { return levelReclaimed; }
        set { levelReclaimed = Mathf.Clamp01(value); }     // should only be used for dev, increase with Reclaim()
    }

    public BuildingStatus Status
    {
        get { return status; }
        set { status = value; }
    }

    public bool IsUndiscovered()
    {
        if (status == BuildingStatus.UNDISCOVERED)
            return true;
        return false;
    }

    public bool IsDiscovered()
    {
        if (!IsUndiscovered())
            return true;
        return false;
    }

    public bool IsOnlyDiscovered()
    {
        if (status == BuildingStatus.DISCOVERED)
            return true;
        return false;
    }

    public bool IsAssessed()
    {
        if (IsOnlyAssessed() || IsReclaimed())
            return true;
        return false;
    }

    public bool IsOnlyAssessed()
    {
        if (status == BuildingStatus.ASSESSED)
            return true;
        return false;
    }

    public bool IsReclaimed()
    {
        if (status == BuildingStatus.RECLAIMED)
            return true;
        return false;
    }
    #endregion
}

public class TaskNotFoundException : Exception
{
    public TaskNotFoundException()
    {
    }

    public TaskNotFoundException(string message) : base(message)
    {
    }

    public TaskNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TaskNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

public class TaskAlreadyAddedException : Exception
{
    public TaskAlreadyAddedException()
    {
    }

    public TaskAlreadyAddedException(string message) : base(message)
    {
    }

    public TaskAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected TaskAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
