using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DBuilding : TurnUpdatable {

    public enum DBuildingStatus
    {
        UNDISCOVERED,
        DISCOVERED,
        ASSESSED,
    }

    private static int NEXT_ID = 0;
    private BuildingController buildingController;

    private int id;
    private DCity city;
    private String buildingName;
    private DBuildingStatus status;
    public Dictionary<int, DTask> tasks = new Dictionary<int, DTask>();
    private DTask_Assess assessTask;

    private float percentInfected;
    private float percentDamaged;
    private float percentAssessed;

    public DBuilding(DCity city, string buildingName, BuildingController buildingController)
    {
        this.id = NEXT_ID++;
        this.city = city;
        this.buildingName = buildingName;
        this.buildingController = buildingController;

        this.status = DBuildingStatus.UNDISCOVERED;
        this.percentAssessed = 0.0f;

        // Add an assess task by default
        this.assessTask = new DTask_Assess(this, 0.2f, 1, "Assess Building");

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
        CalculateDamages();
    }

    public void CalculateDamages()
    {
        int numberOfTasks = 0;
        float totalDamaged = 0.0f;
        float totalInfected = 0.0f;
        int numPeople = 0;
        int cumulativeInfectionLevel = 0;
        foreach (var entry in tasks)
        {
            //calculate %
            totalDamaged += entry.Value.LevelDamaged;
            totalInfected += entry.Value.LevelInfected;
            numberOfTasks++;
            if(entry.Value.SlotList != null)
            foreach (var task in entry.Value.SlotList)
            // foreach (var person in task.Value)
                if(task.Person != null) {
                  cumulativeInfectionLevel += task.Person.Infection;
                  numPeople++;
                }
        }
        if(numPeople != 0)
          cumulativeInfectionLevel /= numPeople;
        if(numberOfTasks != 0)
          totalInfected /= numberOfTasks;
// Arbitrary weighting alert
          percentInfected = Mathf.Clamp(totalInfected
          + (cumulativeInfectionLevel*0.25f),0, 1);

        percentDamaged = totalDamaged / numberOfTasks;
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
            CalculateDamages();
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

    public DBuildingStatus Status
    {
        get { return status; }
    }

    public string StatusAsString
    {
        get
        {
            switch (status)
            {
                case DBuildingStatus.UNDISCOVERED:
                    return "Undiscovered";
                case DBuildingStatus.DISCOVERED:
                    return "Discovered";
                case DBuildingStatus.ASSESSED:
                    return "Assessed";
            }

            return "Unknown";
        }
    }

    public void Discover()
    {
        if (status == DBuildingStatus.UNDISCOVERED)
            status = DBuildingStatus.DISCOVERED;
    }

    public void Assess(float assessAmount)
    {
        percentAssessed = Mathf.Clamp01(percentAssessed + assessAmount);

        if (percentAssessed == 1.0f)
        {
            status = DBuildingStatus.ASSESSED;
            assessTask.DisableTask();
            buildingController.ReorganizeTaskControllers();
        }
    }

    public float LevelAssessed
    {
        get { return percentAssessed; }
    }

    public bool Assessed
    {
        get { return percentAssessed == 1.0f; }
    }

    public float LevelDamaged
    {
      get {
            CalculateDamages();
            return percentDamaged;
          }
    }

    public float LevelInfected
    {
        get {
          CalculateDamages();
          return percentInfected; }
    }

    public bool Damaged
    {
        get { return percentDamaged != 1.0f; }
    }

    public bool Infected
    {
        get { return percentInfected != 1.0f; }
    }
    #endregion
}

#region Exceptions

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

#endregion
