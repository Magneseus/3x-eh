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
    public Dictionary<int, DTask> tasks = new Dictionary<int, DTask>();

    private float percentInfected;
    private float percentDamaged;
    public bool discovered;
    private float percentAssessed;

    public DBuilding(DCity city, string buildingName, BuildingController buildingController)
    {
        this.id = NEXT_ID++;
        this.city = city;
        this.buildingName = buildingName;
        this.buildingController = buildingController;

        //TODO: Change the default discovery to false, as it will be false for almost all buildings
        this.discovered = true;
        this.percentAssessed = 0.0f;
        
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

    private void CalculateDamages()
    {
        int numberOfTasks = 0;
        float totalDamaged = 0.0f;
        float totalInfected = 0.0f;

        foreach (var entry in tasks)
        {
            //calculate %
            totalDamaged += entry.Value.LevelDamaged;
            totalInfected += entry.Value.LevelInfected;
            numberOfTasks++;
        }

        percentInfected = totalInfected / numberOfTasks;
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

    public void Assess(float assessAmount)
    {
        percentAssessed = Mathf.Clamp01(percentAssessed + assessAmount);
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
        get { return percentDamaged; }        
    }

    public float LevelInfected
    {
        get { return percentInfected; }
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
