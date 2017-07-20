using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DTask_Assess : DTask
{
    private float assessAmount;

    public DTask_Assess(DBuilding dBuilding, float assessAmount, int dMaxPeople, string dName) : base(dBuilding, null, dMaxPeople, dName, 0.0f)
    {
        this.assessAmount = assessAmount;

        ForceClean();
        ForceFixed();
    }

    public DTask_Assess(DBuilding dBuilding) : this(dBuilding, Constants.DEFAULT_ASSESS_AMOUNT, 4, "Assess")
    {}

    public override void TurnUpdate(int numDaysPassed)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.TurnUpdate(numDaysPassed);

            // TODO: Make this into a exponential scale or something
            if (taskSlot.IsFunctioning())
            {
                float modifier = taskSlot.Person.Infection == Constants.MERSON_INFECTION_MIN ? 1 : Constants.MERSON_INFECTION_TASK_MODIFIER;                                
                building.Assess(assessAmount * Constants.MERSON_INFECTION_TASK_MODIFIER);
            }
        }
    }

    #region Accessors

    public float AssessAmount
    {
        get { return assessAmount; }
        set { assessAmount = Mathf.Clamp01(value); }
    }

    #endregion
}
