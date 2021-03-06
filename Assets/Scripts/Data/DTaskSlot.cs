﻿using System.Runtime.Serialization;
using UnityEngine;
using SimpleJSON;


public class DTaskSlot : ITurnUpdatable
{
    private DTask task;
    private DPerson person;
	public TaskTraySingle taskTraySlot;
    private float structuralDamage;
    private float fungalDamage;
    private bool taskSlotEnabled;
    private bool lockedIn;
    private int numTurnsPassed;

    public DTaskSlot(DTask dTask, DPerson dPerson=null)
    {
        this.task = dTask;
        this.person = dPerson;

        structuralDamage = Random.Range(Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG);
        fungalDamage = Random.Range(Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG);

        taskSlotEnabled = true;
        lockedIn = false;
        numTurnsPassed = 0;
    }

    public void TurnUpdate(int numDaysPassed)
    {
        if (person == null)
            return;

        // Repairing the slot is first and foremost
        if ((Infected || Damaged))
        {
            float modifier = person.Infection == Constants.MERSON_INFECTION_MIN ? 1 : Constants.MERSON_INFECTION_TASK_MODIFIER;
            Repair(Constants.TEMP_REPAIR_AMOUNT * modifier);
        }
        // Other behavior
        else
        {
            // Locking slot
            if (task.NumTurnsToComplete > 0)
            {
                // Start the lock
                if (!lockedIn)
                {
                    LockSlot();
                }

                // Increment turn counter
                numTurnsPassed++;

                // Check if the lock is over
                if (lockedIn && numTurnsPassed >= task.NumTurnsToComplete)
                {
                    UnlockSlot();
                }
            }
        }
    }

    public void LockSlot()
    {
        if (!lockedIn && person != null)
        {
            lockedIn = true;
            numTurnsPassed = 0;

            person.LockMeeple();
        }
    }

    public void UnlockSlot()
    {
        lockedIn = false;

        if (person != null)
            person.UnlockMeeple();
    }

    public void StructureDeteriorates()
    {
        structuralDamage = Mathf.Clamp(structuralDamage * DSeasons.changeStructureDamageSlots[(int)DSeasons._season.WINTER], 0f, 1f);
    }

    public void FungusGrows()
    {
        fungalDamage = Mathf.Clamp(fungalDamage * DSeasons.changeFungusSlots[(int)DSeasons._season.WINTER], 0f, 1f);
    }

    #region Person Management

    public void AddPerson(DPerson dPerson)
    {
        if (person == null && taskSlotEnabled)
        {
         	person = dPerson;
			if(dPerson.Task != null)
				dPerson.Task.RemovePerson(dPerson);

            dPerson.__TaskSlot(this);
            task.RaisePersonCount();
        }
        else
        {
            throw new TaskSlotFullException(task.Name + " Person ID: " + person.ID);
        }
    }

    public void RemovePerson()
    {
        if (person != null)
		{
            UnlockSlot();

            person.__TaskSlot(null);
            person = null;
            task.LowerPersonCount();
        }
    }

    public void MoveToTownHall()
    {
        if (person != null)
        {
            UnlockSlot();
            person.MoveToTownHall();

            person = null;
            task.LowerPersonCount();
        }
    }

    #endregion

    public void Repair(float amount)
    {
        if (Infected)
        {
            fungalDamage -= amount * DSeasons.modRepairFungusSpeed[(int)FetchSeason()];
            fungalDamage = Mathf.Clamp(fungalDamage, Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG);
        }
        else if (Damaged)
        {
            structuralDamage -= amount * DSeasons.modRepairStructureSpeed[(int)FetchSeason()]; ;
            structuralDamage = Mathf.Clamp(structuralDamage, Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG);
        }
    }

    public bool IsFunctioning()
    {
        if (!Infected && !Damaged && person != null && taskSlotEnabled)
            return true;

        return false;
    }

    private DSeasons._season FetchSeason()
    {
        return task.Building.City.Season;
    }

    #region Accessors

    public DPerson Person
    {
        get { return person; }
		set { person = value;}
    }

    public DTask Task
    {
        get { return task; }
    }

    public float LevelDamaged
    {
        get { return structuralDamage; }
        set { structuralDamage = Mathf.Clamp(value, Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG); }
    }

    public float LevelInfected
    {
        get { return fungalDamage; }
        set { fungalDamage = Mathf.Clamp(value, Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG); }
    }

    public bool Damaged
    {
        get { return structuralDamage != Constants.TASK_MIN_STRUCTURAL_DMG; }
    }

    public bool Infected
    {
        get { return fungalDamage != Constants.TASK_MIN_FUNGAL_DMG; }
    }

    public bool Enabled
    {
        get { return taskSlotEnabled; }
        set { taskSlotEnabled = value; }
    }

	public TaskTraySingle TaskTraySlot
	{
		get { return taskTraySlot; }
		set { taskTraySlot = value; }
	}

    public bool IsLocked
    {
        get { return lockedIn; }
    }

    public int NumTurnsPassed
    {
        get { return numTurnsPassed; }
    }

    #endregion

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save task info
        returnNode.Add("taskName", new JSONString(task.Name));
        returnNode.Add("taskSlotEnabled", new JSONBool(taskSlotEnabled));
        returnNode.Add("lockedIn", new JSONBool(lockedIn));
        returnNode.Add("numTurnsPassed", new JSONNumber(numTurnsPassed));

        // Save damage
        returnNode.Add("structuralDamage", new JSONNumber(structuralDamage));
        returnNode.Add("fungalDamage", new JSONNumber(fungalDamage));

        // Save person id
        if (person != null)
            returnNode.Add("personID", new JSONNumber(person.ID));
        else
            returnNode.Add("personID", new JSONNull());

        return returnNode;
    }

    public static DTaskSlot LoadFromJSON(JSONNode jsonNode, DTask task)
    {
        DTaskSlot returnTaskSlot = new DTaskSlot(task);

        // Load slot info
        returnTaskSlot.taskSlotEnabled = jsonNode["taskSlotEnabled"].AsBool;
        returnTaskSlot.lockedIn = jsonNode["lockedIn"].AsBool;
        returnTaskSlot.numTurnsPassed = RandJSON.JSONInt(jsonNode["numTurnsPassed"], 0);

        // Load damage
        returnTaskSlot.structuralDamage = RandJSON.JSONFloat(jsonNode["structuralDamage"]);
        returnTaskSlot.fungalDamage = RandJSON.JSONFloat(jsonNode["fungalDamage"]);

        // Find and add person
        if (!jsonNode["personID"].IsNull)
        {
            DCity city = task.Building.City;
            foreach (var person in city.People)
            {
                if (person.Value.ID == jsonNode["personID"].AsInt)
                {
                    person.Value.SetTaskSlot(returnTaskSlot);
                }
            }
        }

        return returnTaskSlot;
    }
}

#region Exceptions

public class TaskSlotFullException : System.Exception
{
    public TaskSlotFullException()
    {
    }

    public TaskSlotFullException(string message) : base(message)
    {
    }

    public TaskSlotFullException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected TaskSlotFullException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
