using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPerson : TurnUpdatable
{
    private static int NEXT_ID = 0;
    
    private int id;
    private DCity city;
    private DTask task;

    public DPerson(DCity dCity)
    {
        id = NEXT_ID++;
        city = dCity;

        city.AddPerson(this);
    }
       
    public void TurnUpdate(int numDaysPassed)
    {

    }

    public void SetTask(DTask dTask)
    {
        if(task != null)
            task.RemovePerson(this);

        task = dTask;

        if (!dTask.ListOfPeople.Contains(this))
            dTask.AddPerson(this);
    }

    public void RemoveTask(DTask dTask)
    {
        if (task != null)
        {
            if (task.ListOfPeople.Contains(this))
            {
                task.RemovePerson(this);
            }

            task = null;
        }
        else
        {
            throw new TaskNotFoundException("Person ID: " + id + " Task: " + dTask.Name);
        }
    }

    public void ClearTask()
    {
        RemoveTask(task);
    }
        

    public int ID
    {
        get { return id; }
    }

    public DTask Task
    {
        get { return task; }
        set { task = value; }
    }

    public DCity City
    {
        get { return city; }
    }
}
