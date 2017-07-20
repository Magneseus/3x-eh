using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DTask : TurnUpdatable
{

    private static int NEXT_ID = 0;

    protected int id;
    protected string taskName;
    protected DBuilding building;

    protected int maxPeople;
    protected int numPeople;
    protected List<DTaskSlot> slotList;

    protected float fullAssessRequirement;
    protected DResource output;
    protected bool taskEnabled;


    public DTask(DBuilding dBuilding, DResource dOutput, int dMaxPeople, string dName, float dFullAssessRequirement)
    {
        slotList = new List<DTaskSlot>();

        id = NEXT_ID++;
        taskName = dName;
        building = dBuilding;
        output = dOutput;
        maxPeople = dMaxPeople;
        numPeople = 0;
        fullAssessRequirement = dFullAssessRequirement;

        CalculateAssessmentLevels();

        for (int i = 0; i < dMaxPeople; i++)
        {
            // Create all task slots
            slotList.Add(new DTaskSlot(this));
        }

        taskEnabled = true;
        dBuilding.AddTask(this);
    }

    public DTask(DBuilding dBuilding, DResource dOutput) : this(dBuilding, dOutput, 4, "default_task", 0.0f)
    {
    }

    public virtual void TurnUpdate(int numDaysPassed)
    {
        CalculateAssessmentLevels();

        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.TurnUpdate(numDaysPassed);

            if (taskSlot.IsFunctioning())
            {
                float modifier = taskSlot.Person.Infection == Constants.MERSON_INFECTION_MIN ? 1 : Constants.MERSON_INFECTION_TASK_MODIFIER;
                building.OutputResource(DResource.Create(output, Mathf.RoundToInt(output.Amount * modifier)));
            }

        }
    }

    #region Person Management

    public void AddPerson(DPerson dPerson)
    {
        if (numPeople >= maxPeople)
        {
            throw new TaskFullException(taskName);
        }
        else if (ContainsPerson(dPerson))
        {
            throw new PersonAlreadyAddedException(taskName);
        }
        else
        {
            foreach (DTaskSlot taskSlot in slotList)
            {
                if (taskSlot.Person == null && taskSlot.Enabled)
                {
                    if (dPerson.Task != null)
                        dPerson.RemoveTask();

                    taskSlot.AddPerson(dPerson);
                    return;
                }
            }
        }
    }

    public void RemovePerson(DPerson dPerson)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            if (taskSlot.Person == dPerson)
            {
                taskSlot.RemovePerson();
                numPeople--;

                return;
            }
        }

        throw new PersonNotFoundException(taskName);
    }

    public bool ContainsPerson(DPerson dPerson)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            if (taskSlot.Person == dPerson)
                return true;
        }

        return false;
    }

    public void RaisePersonCount()
    {
        numPeople++;
    }

    public void LowerPersonCount()
    {
        numPeople--;
    }

    #endregion

    public void EnableTask()
    {
        taskEnabled = true;
    }

    public void DisableTask()
    {
        // Remove people from task
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.RemovePerson();
        }

        // Disable task
        taskEnabled = false;
    }

    public void ForceClean()
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.LevelInfected = Constants.TASK_MIN_FUNGAL_DMG;
        }
    }

    public void ForceFixed()
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.LevelDamaged = Constants.TASK_MIN_STRUCTURAL_DMG;
        }
    }

    public DTaskSlot GetTaskSlot(int index)
    {
        return slotList[index];
    }

    public int CalculateAssessmentLevels()
    {
        if (fullAssessRequirement == 0.0f)
            return maxPeople;

        int numEnabled = Mathf.FloorToInt(Mathf.Clamp01(building.LevelAssessed / fullAssessRequirement) * (float)maxPeople);
        Debug.Log(taskName + " : " + numEnabled);

        for (int i = 0; i < slotList.Count; i++)
        {
            if (i <= numEnabled-1)
            {
                slotList[i].Enabled = true;
            }
            else
            {
                slotList[i].Enabled = false;
            }
        }

        return numEnabled;
    }

    #region Properties
    public float LevelDamaged
    {
        get
        {
            float avgDamage = 0.0f;
            foreach (DTaskSlot taskSlot in slotList)
            {
                avgDamage += taskSlot.LevelDamaged;
            }
            avgDamage /= maxPeople;

            return avgDamage;
        }
    }

    public float LevelInfected
    {
        get
        {
            float avgInfection = 0.0f;
            foreach (DTaskSlot taskSlot in slotList)
            {
                avgInfection += taskSlot.LevelInfected;
            }
            avgInfection /= maxPeople;

            return avgInfection;
        }
    }

    public bool Damaged
    {
        get { return LevelDamaged != Constants.TASK_MIN_STRUCTURAL_DMG; }
    }

    public bool Infected
    {
        get { return LevelInfected != Constants.TASK_MIN_FUNGAL_DMG; }
    }

    public int NumPeople
    {
        get { return numPeople; }
    }

    public int MaxPeople
    {
        get { return maxPeople; }
    }

    public string Name
    {
        get { return taskName; }
        set { taskName = value; }
    }

    public int ID
    {
        get { return id; }
    }

    public DResource Output
    {
        get { return output; }
    }

    public DBuilding Building
    {
        get { return building; }
    }

    public bool Enabled
    {
        get { return taskEnabled; }
    }
    public List<DTaskSlot> SlotList
    {
      get {return slotList;}
    }
    #endregion
}


#region Exceptions

public class TaskFullException : System.Exception
{
    public TaskFullException()
    {
    }

    public TaskFullException(string message) : base(message)
    {
    }

    public TaskFullException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected TaskFullException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
