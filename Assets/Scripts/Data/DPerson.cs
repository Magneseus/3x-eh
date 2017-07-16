﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPerson : TurnUpdatable
{
    private static int NEXT_ID = 0;
    private MeepleController meepleController;
    
    private int id;
    private DCity city;
    private DTaskSlot taskSlot;

    public DPerson(DCity dCity, MeepleController mController)
    {
        id = NEXT_ID++;
        city = dCity;
        meepleController = mController;
        taskSlot = null;

        city.AddPerson(this);
    }
       
    public void TurnUpdate(int numDaysPassed)
    {

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

            //MoveToTownHall();
        }
        else
        {
            throw new TaskNotFoundException("Person ID: " + id);
        }
    }

    public void MoveToTownHall()
    {
        //TODO: When the empty building is made / townhall, move to that instead of global parent

	DTask townHallIdle = city.Buildings[0].Tasks[1];
	Debug.Log(townHallIdle.Name);
	townHallIdle.AddPerson(this);
	meepleController.SetParentTrayAndTransfrom(taskSlot.TaskTraySlot);

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

    public DCity City
    {
        get { return city; }
    }

    #endregion
}
