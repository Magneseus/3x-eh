using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System;
public class DPerson : TurnUpdatable
{
    private static int NEXT_ID = 0;
    private MeepleController meepleController;

    private int id;
    private int infectionLevel;
    private DCity city;
    private DBuilding building;
    private DTaskSlot taskSlot;

    public DPerson(DCity dCity, MeepleController mController)
    {
        id = NEXT_ID++;
        city = dCity;
        meepleController = mController;
        taskSlot = null;
        infectionLevel = 0;

        city.AddPerson(this);
    }

    public void TurnUpdate(int numDaysPassed)
    {
      // Random random = new Random();
      if(building != null && Random.value <= (building.LevelInfected +  Constants.MERSON_INFECTION_PROBABILITY))
      IncreaseInfection();
      Debug.Log("Merson: "+ id +" infectionLevel is -> "+ infectionLevel );
    }

    #region Infection
    public void IncreaseInfection()
    {
        infectionLevel = Mathf.Clamp(++infectionLevel, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX);
        if(infectionLevel == 2)
        city.Health = Mathf.Clamp((city.Health -0.1f), Constants.CITY_MIN_HEALTH,Constants.CITY_MAX_HEALTH);
    }

    public void DecreaseInfection()
    {
        infectionLevel = Mathf.Clamp(--infectionLevel, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX);
        if(infectionLevel == 1)
        city.Health = Mathf.Clamp((city.Health +0.1f), Constants.CITY_MIN_HEALTH,Constants.CITY_MAX_HEALTH);
    }
    #endregion

    #region Task Management

    public void SetTask(DTask dTask)
    {
        if (taskSlot != null && Task != dTask)
            RemoveTask();

        dTask.AddPerson(this);
    }

    public void SetTaskSlot(DTaskSlot dTaskSlot)
    {
        if (taskSlot != null)
            RemoveTask();

        dTaskSlot.AddPerson(this);
    }

    public void __TaskSlot(DTaskSlot dtaskSlot)
    {
        taskSlot = dtaskSlot;

        if (taskSlot == null)
        {
            MoveToTownHall();
        }
    }

    public void RemoveTask()
    {
        if (taskSlot != null)
        {
            taskSlot.RemovePerson();
            taskSlot = null;

            MoveToTownHall();
        }
        else
        {
            throw new TaskNotFoundException("Person ID: " + id);
        }
    }

    public void MoveToTownHall()
    {
        //TODO: When the empty building is made / townhall, move to that instead of global parent
        meepleController.transform.parent = null;
        meepleController.gameObject.SetActive(true);
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
        get { return infectionLevel; }
        set { infectionLevel = Mathf.Clamp(value, Constants.MERSON_INFECTION_MIN, Constants.MERSON_INFECTION_MAX); }
    }

    #endregion
}
