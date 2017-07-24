using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
// using System;

public class DPerson : TurnUpdatable, JSONParsable<DPerson>
{
    private static int NEXT_ID = 0;
    private MeepleController meepleController;

    private int id;
    private int infectionLevel;
    private DCity city;
    private DBuilding building;
    private DTaskSlot taskSlot;
    private bool isDead;

    public DPerson(DCity dCity, MeepleController mController)
    {
        id = NEXT_ID++;
        city = dCity;
        meepleController = mController;
        taskSlot = null;
        infectionLevel = 0;
        isDead = false;

        city.AddPerson(this);
    }

    public void TurnUpdate(int numDaysPassed)
    {
      // Random random = new Random();
      if(building != null && Random.value <= (building.LevelInfected +  Constants.MERSON_INFECTION_PROBABILITY))
      IncreaseInfection();
    }

    #region Infection
    public void IncreaseInfection()
    {
        infectionLevel = Mathf.Clamp(++infectionLevel, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX);
    }

    public void DecreaseInfection()
    {
        infectionLevel = Mathf.Clamp(--infectionLevel, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX);
    }
    #endregion

    public void Dies()
    {
        Object.Destroy(meepleController.gameObject);
        isDead = true;
    }

    #region Task Management

    public void SetTask(DTask dTask)
    {
        if (taskSlot != null && Task != dTask)
            RemoveTask();

        dTask.AddPerson(this);
    }

    public void SetTaskSlot(DTaskSlot dTaskSlot)
    {
		if(taskSlot != null)
			Task.RemovePerson(this);
		if(dTaskSlot.Task==city.townHall.getIdleTask())
			((DTask_Idle)dTaskSlot.Task).AddPerson(this, dTaskSlot);
        else if (dTaskSlot.Task.Name.Equals("Explore"))
            ((DTask_Explore)dTaskSlot.Task).AddPerson(this, dTaskSlot);
        else
        	dTaskSlot.AddPerson(this);
    }

    public void __TaskSlot(DTaskSlot dtaskSlot)
    {
        taskSlot = dtaskSlot;
    }

    public void RemoveTask()
    {
        if (taskSlot != null)
        {
            taskSlot.RemovePerson();
            taskSlot = null;
        }
        else
        {
            throw new TaskNotFoundException("Person ID: " + id);
        }
    }


	public void MoveToTownHall()
    {
        // Move to town hall in data
		if(Task !=null)
        	RemoveTask();
        city.townHall.getIdleTask().AddPerson(this);

        meepleController.SetParentTrayAndTransfrom(taskSlot.TaskTraySlot);
    }

    



    #endregion

    #region Properties

    public int ID
    {
        get { return id; }
    }

    public DTask Task
    {
        get{ return taskSlot == null ? null : taskSlot.Task; }
    }

    public DTaskSlot TaskSlot
    {
        get { return taskSlot; }
    }
    public DBuilding Building
    {
        get { return building;}
        set { building = value;}
    }
    public DCity City
    {
        get { return city; }
    }

    public int Infection
    {
        get { return Mathf.Min(infectionLevel, Constants.MERSON_SEASON_INFECTION_MAX[(int)city.Season]); }
        set { infectionLevel = Mathf.Clamp(value, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX); }
    }

    public int InfectionActual
    {
        get { return infectionLevel; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    #endregion

    public JSONNode SaveToJSON()
    {
        throw new System.NotImplementedException();
    }

    public DPerson LoadFromJSON(JSONNode jsonNode)
    {
        throw new System.NotImplementedException();
    }
}
