using System.Runtime.Serialization;
using UnityEngine;

public class DTaskSlot : TurnUpdatable
{
    private DTask task;
    private DPerson person;
	private TaskTraySingle taskTraySlot;
    private float structuralDamage;
    private float fungalDamage;
    private bool taskSlotEnabled;

    public DTaskSlot(DTask dTask, DPerson dPerson=null)
    {
        this.task = dTask;
        this.person = dPerson;

        structuralDamage = Random.Range(Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG);
        fungalDamage = Random.Range(Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG);

        taskSlotEnabled = true;
    }

    public void TurnUpdate(int numDaysPassed)
    {
        if (person != null)
        {
            if (Infected)
                Repair(Constants.TEMP_REPAIR_AMOUNT);
            else if (Damaged)
                Repair(Constants.TEMP_REPAIR_AMOUNT);
        }
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
            person.__TaskSlot(null);
            person = null;
			Debug.Log(task.Name+": "+task.NumPeople);
            task.LowerPersonCount();
        }
		Debug.Log(task.Name+": "+task.NumPeople);
    }

    #endregion

    public void Repair(float amount)
    {
        if (Infected)
        {
            fungalDamage -= amount;
            fungalDamage = Mathf.Clamp(fungalDamage, Constants.TASK_MIN_FUNGAL_DMG, Constants.TASK_MAX_FUNGAL_DMG);
        }
        else if (Damaged)
        {
            structuralDamage -= amount;
            structuralDamage = Mathf.Clamp(structuralDamage, Constants.TASK_MIN_STRUCTURAL_DMG, Constants.TASK_MAX_STRUCTURAL_DMG);
        }
    }

    public bool IsFunctioning()
    {
        if (!Infected && !Damaged && person != null && taskSlotEnabled)
            return true;

        return false;
    }

    #region Accessors

    public DPerson Person
    {
        get { return person; }
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

    #endregion
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