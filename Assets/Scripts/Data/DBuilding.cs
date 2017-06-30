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

    public DBuilding(DCity city, string buildingName, BuildingController buildingController)
    {
        this.id = NEXT_ID++;
        this.city = city;        
        this.buildingName = buildingName;
        this.buildingController = buildingController;

        city.AddBuilding(this);
    }

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        foreach(var entry in tasks)
            entry.Value.TurnUpdate(numDaysPassed);
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

