using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleJSON;
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

    private DTask_Explore exploreTask;
    private DTask_Idle idleTask;
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

		if(buildingName.Equals("Town Hall")) {
			this.idleTask = new DTask_Idle(this, "Idle Merson");
            this.exploreTask = new DTask_Explore(this, 0.1f, "Explore");
        } 
		
		this.assessTask = new DTask_Assess (this, 0.2f, 1, "Assess Building");

        percentDamaged = 0.0f;
        percentInfected = 0.0f;

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

        if ((status == DBuildingStatus.UNDISCOVERED))
            buildingController.gameObject.SetActive(false);
        else
            buildingController.gameObject.SetActive(true);

    }

  
    public void SpringEffects()
    {

    }

    public void SummerEffects()
    {
        FungusGrows();
    }

    public void FallEffects()
    {

    }

    public void WinterEffects()
    {
        StructureDeteriorates();
    }

    public void StructureDeteriorates()
    {
        foreach (var entry in tasks)
            entry.Value.StructureDeteriorates();
    }

    public void FungusGrows()
    {
        foreach (var entry in tasks)
            entry.Value.FungusGrows();
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
          percentInfected = Mathf.Clamp(totalInfected
          + (cumulativeInfectionLevel*Constants.BUILDING_MERSON_INFECTION_WEIGHT),Constants.BUILDING_MIN_FUNGAL_DMG, Constants.BUILDING_MAX_FUNGAL_DMG);

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

	public DTask_Idle getIdleTask()
	{
        return idleTask;	
	}
    public DTask_Explore getExploreTask()
    {
        return exploreTask;
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

	public BuildingController Controller
	{
		get { return buildingController; }
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

    public float SeasonalInfectionMod()
    {
        return DSeasons.modBuildingInfection[(int)city.Season];
    }

    public float LevelAssessed
    {
        get { return percentAssessed; }
        set { percentAssessed = value; }
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
        get
        {
          CalculateDamages();
          return Mathf.Clamp(percentInfected * SeasonalInfectionMod(), 0f, 1f);
        }
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

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save basic building info
        returnNode.Add("name", new JSONString(buildingName));
        returnNode.Add("ID", new JSONNumber(id));
        returnNode.Add("cityName", new JSONString(city.Name));

        // Save status information
        returnNode.Add("status", new JSONNumber((int)(status)));
        returnNode.Add("percentInfected", new JSONNumber(percentInfected));
        returnNode.Add("percentDamaged", new JSONNumber(percentDamaged));
        returnNode.Add("percentAssessed", new JSONNumber(percentAssessed));

        // Save tasks
        JSONArray jsonTaskList = new JSONArray();
        foreach (var task in tasks)
        {
            jsonTaskList.Add(task.Value.SaveToJSON());
        }
        returnNode.Add("tasks", jsonTaskList);

        return returnNode;
    }

    public static DBuilding LoadFromJSON(JSONNode jsonNode, DCity city, BuildingController buildingController)
    {
        DBuilding dBuilding = new DBuilding(city, jsonNode["name"], buildingController);

        // Load basic info
        dBuilding.id = jsonNode["ID"].AsInt;

        // Load status info
        dBuilding.status = (DBuildingStatus)(jsonNode["status"].AsInt);
        dBuilding.percentInfected = jsonNode["percentInfected"].AsFloat;
        dBuilding.percentDamaged = jsonNode["percentDamaged"].AsFloat;
        dBuilding.percentAssessed = jsonNode["percentAssessed"].AsFloat;

        // Load tasks
        foreach (JSONNode taskNode in jsonNode["tasks"].AsArray)
        {
            dBuilding.AddTask(DTask.LoadFromJSON(taskNode));
        }

        return dBuilding;
    }
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
