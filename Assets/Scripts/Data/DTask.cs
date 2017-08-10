using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleJSON;
using UnityEngine;

public class DTask : ITurnUpdatable
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
    protected DResource input;
    protected bool taskEnabled;
    protected int numTurnsToComplete;


    public DTask(DBuilding dBuilding, DResource dOutput, int dMaxPeople, string dName, float dFullAssessRequirement, DResource dInput = null)
    {
        slotList = new List<DTaskSlot>();

        id = NEXT_ID++;
        taskName = dName;
        building = dBuilding;
        output = dOutput;
        input = dInput;
        maxPeople = dMaxPeople;
        numPeople = 0;
        fullAssessRequirement = dFullAssessRequirement;
        numTurnsToComplete = 0;

        CalculateAssessmentLevels();

        for (int i = 0; i < dMaxPeople; i++)
        {
            // Create all task slots
            slotList.Add(new DTaskSlot(this));
        }
        if (taskName.Equals("Treat People"))
        {
            output = null;
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
                bool canOutputResource = true;

                // Check slot lock
                if (!(taskSlot.NumTurnsPassed >= numTurnsToComplete))
                    canOutputResource = false;

                // Process resource consumption
                if (canOutputResource && input != null)
                {
                    if (building.City.GetResource(input.Name).Amount >= input.Amount)
                        ConsumeResources();
                    else
                        canOutputResource = false;
                }

                // Resource production
                if (canOutputResource)
                    ProduceResources(taskSlot);
            }

        }
    }

    private void ProduceResources(DTaskSlot taskSlot)
    {
        float modifier = taskSlot.Person.Infection == Constants.MERSON_INFECTION_MIN ? 1 : Constants.MERSON_INFECTION_TASK_MODIFIER;
        building.OutputResource(DResource.Create(output, Mathf.RoundToInt(output.Amount * modifier)));
        if (taskName.Equals("Treat People"))
        {
            RandomalyTreatPeople();
        }
    }

    private void ConsumeResources()
    {
        building.InputResource(input);
    }

    public void StructureDeteriorates()
    {
        foreach (var entry in slotList)
            entry.StructureDeteriorates();
    }

    public void FungusGrows()
    {
        foreach (var entry in slotList)
            entry.FungusGrows();
    }

    #region Person Management

    public virtual void AddPerson(DPerson dPerson)
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

    public virtual void RemovePerson(DPerson dPerson)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            if (taskSlot.Person == dPerson)
            {
                taskSlot.RemovePerson();

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

    public DPerson lastPerson()
    {
        DPerson last = null;
        foreach (DTaskSlot slot in slotList)
        {
            if (slot.Person != null)
                last = slot.Person;

        }
        return last;
    }

    public void DisableTask()
    {
        // Remove people from task
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.MoveToTownHall();
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

        for (int i = 0; i < slotList.Count; i++)
        {
            if (i <= numEnabled - 1)
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
    public void RandomalyTreatPeople()
    {
        if (building.City.People.Count > 0)
        {
            int index = Random.Range(0, building.City.People.Count - 1);
            building.City.People[index].DecreaseInfection();
        }
    }

    public virtual JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save Task info
        returnNode.Add("ID", new JSONNumber(id));
        returnNode.Add("name", new JSONString(taskName));
        returnNode.Add("buildingName", new JSONString(building.Name));

        // Save person info
        returnNode.Add("maxPeople", new JSONNumber(maxPeople));
        returnNode.Add("numPeople", new JSONNumber(numPeople));

        // Save output info
        returnNode.Add("fullAssessRequirement", new JSONNumber(fullAssessRequirement));
        returnNode.Add("taskEnabled", new JSONBool(taskEnabled));
        returnNode.Add("numTurnsToComplete", new JSONNumber(numTurnsToComplete));

        // Resource output
        if (output != null)
            returnNode.Add("resourceOutput", output.SaveToJSON());
        else
            returnNode.Add("resourceOutput", new JSONNull());

        // Resource input
        if (input != null)
            returnNode.Add("resourceInput", input.SaveToJSON());
        else
            returnNode.Add("resourceInput", new JSONNull());

        // Save task slot list
        JSONArray jsonSlotList = new JSONArray();
        foreach (var taskSlot in slotList)
        {
            jsonSlotList.Add(taskSlot.SaveToJSON());
        }
        returnNode.Add("taskSlots", jsonSlotList);

        return returnNode;
    }

    public static DTask LoadFromJSON(JSONNode jsonNode, DBuilding building)
    {
        DTask returnTask = null;

        // Check for special task cases
        if (jsonNode["resourceOutput"].IsNull)
        {
            if (jsonNode["specialTask"] == "assess")
            {
                returnTask = new DTask_Assess(
                    building,
                    RandJSON.JSONFloat(jsonNode["assessAmount"]),
                    RandJSON.JSONInt(jsonNode["maxPeople"]),
                    jsonNode["name"]);
            }
            else if (jsonNode["specialTask"] == "explore")
            {
                returnTask = new DTask_Explore(
                    building,
                    RandJSON.JSONFloat(jsonNode["exploreAmount"]),
                    jsonNode["name"]);
            }
            else if (jsonNode["specialTask"] == "idle")
            {
                returnTask = new DTask_Idle(building, jsonNode["name"]);
            }
        }
        else
        {
            returnTask = new DTask(
                building,
                DResource.LoadFromJSON(jsonNode["resourceOutput"]),
                RandJSON.JSONInt(jsonNode["maxPeople"]),
                jsonNode["name"],
                RandJSON.JSONFloat(jsonNode["fullAssessRequirement"]),
                DResource.LoadFromJSON(jsonNode["resourceInput"]));
        }

        // Set the other vars
        if (returnTask == null)
        {
            throw new NullTaskException("Task failed to load from JSON: " + jsonNode["name"]);
        }
        else
        {
            // Load task info
            returnTask.id = jsonNode["ID"].AsInt;

            // Load person info
            returnTask.maxPeople = RandJSON.JSONInt(jsonNode["maxPeople"]);

            // Save output info
            returnTask.fullAssessRequirement = RandJSON.JSONFloat(jsonNode["fullAssessRequirement"]);
            returnTask.taskEnabled = jsonNode["taskEnabled"].AsBool;
            returnTask.NumTurnsToComplete = RandJSON.JSONInt(jsonNode["numTurnsToComplete"], 0);

            // Load the task slots
            returnTask.slotList = new List<DTaskSlot>();
            foreach (JSONNode taskSlotJSON in jsonNode["taskSlots"].AsArray)
            {
                returnTask.SlotList.Add(DTaskSlot.LoadFromJSON(taskSlotJSON, returnTask));


            }

            // Verify that the number of people is correct
            if (returnTask.numPeople  != jsonNode["numPeople"].AsInt)
            {
              // Debug.Log (returnTask.numPeople+ ":" + jsonNode["numPeople"]);

                // throw new TaskLoadException("Num people does not match.");
            }
        }

        return returnTask;
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

    public DResource Input
    {
        get { return input; }
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
        get { return slotList; }
    }

    public int NumTurnsToComplete
    {
        get { return numTurnsToComplete; }
        set { numTurnsToComplete = value; }
    }

    #endregion

    public override string ToString()
    {
        if (taskEnabled && CalculateAssessmentLevels() > 0)
        {
            string text = taskName + ": " + numPeople + " / " + CalculateAssessmentLevels() + ",\t turns: " + numTurnsToComplete + ", + " + (output.Amount*numPeople) + " " + output.Name;
            return text;
        }
        return "";
    }
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

public class TaskLoadException : System.Exception
{
    public TaskLoadException()
    {
    }

    public TaskLoadException(string message) : base(message)
    {
    }

    public TaskLoadException(string message, System.Exception innerException) : base(message, innerException)
    {
    }

    protected TaskLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

#endregion
