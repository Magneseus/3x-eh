using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


public class DTask_Explore : DTask
{
    float exploreAmount;

    public DTask_Explore(DBuilding dBuilding, float exploreAmount, string dName) : base(dBuilding, null, 1, dName, 1.0f)
    {
        this.exploreAmount = exploreAmount;
        ForceClean();
        ForceFixed();
        Reszie();
    }

    public override void TurnUpdate(int numDaysPassed)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.TurnUpdate(numDaysPassed);
                        
            // TODO: Make this into a exponential scale or something
            if (taskSlot.IsFunctioning())
            {
                float infectionModifier = taskSlot.Person.Infection == Constants.MERSON_INFECTION_MIN ? 1f : Constants.MERSON_INFECTION_TASK_MODIFIER;

                if (taskName == "Explore")
                {
                    building.City.Explore(exploreAmount * infectionModifier);
                }
                else if (taskName == "Scavenge")
                {
                    // TODO: Once random events are in place, proper scavenge task here
                    float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                    int randAmt = Mathf.RoundToInt(UnityEngine.Random.Range(10.0f, 50.0f) * infectionModifier);

                    if (rand > 0.4f && rand < 0.7f)
                    {
                        building.OutputResource(DResource.Create("Materials", randAmt));
                    }
                    else if (rand >= 0.7f && rand < 0.9f)
                    {
                        building.OutputResource(DResource.Create("Fuel", randAmt));
                    }
                    else if (rand >= 0.9f)
                    {
                        building.OutputResource(DResource.Create("Medicine", randAmt));
                    }
                }
            }
        }

        // If we've finished exploring, convert to Scavenge task
        if (building.City.CalculateExploration() == 1.0f)
        {
            this.taskName = "Scavenge";
        }
    }

    public void AddPerson(DPerson dPerson, DTaskSlot taskSlot)
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
            if (taskSlot.Person == null && taskSlot.Enabled)
            {
                if (dPerson.Task != null)
                    dPerson.RemoveTask();

                taskSlot.AddPerson(dPerson);
                Reszie();
                return;
            }
        }
    }

    public override void RemovePerson(DPerson dPerson)
    {
        base.RemovePerson(dPerson);
        Reszie();

    }
    public override void AddPerson(DPerson dPerson)
    {
        base.AddPerson(dPerson);
        Reszie();
    }


    private void Reszie()
    {
        if (numPeople == maxPeople)
            AddSlot();
        if (maxPeople - numPeople >= 2)
            RemoveSlot();
    }

    private void AddSlot()
    {
        maxPeople++;
        slotList.Add(new DTaskSlot(this));
        ForceClean();
        ForceFixed();
    }

    private void RemoveSlot()
    {
        maxPeople--;
        if (slotList[slotList.Count - 1].Person == null)
        {
            slotList.RemoveAt(slotList.Count - 1);
        }
    }
}
