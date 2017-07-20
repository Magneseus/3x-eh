using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPerson : TurnUpdatable
{
    private static int NEXT_ID = 0;
    private MeepleController meepleController;
    
    private int id;
    private int infectionLevel;
    private DCity city;
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
