using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DTask_Assess : DTask
{
    public const float DEFAULT_ASSESS_AMOUNT = 0.1f;
    private float assessAmount;

    public DTask_Assess(DBuilding dBuilding, float assessAmount, int dMaxPeople, string dName) : base(dBuilding, null, dMaxPeople, dName)
    {
        this.assessAmount = assessAmount;

        ForceClean();
        ForceFixed();
    }

    public DTask_Assess(DBuilding dBuilding) : this(dBuilding, DEFAULT_ASSESS_AMOUNT, 4, "Assess")
    {}

    public override void TurnUpdate(int numDaysPassed)
    {
        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.TurnUpdate(numDaysPassed);

            // TODO: Make this into a exponential scale or something
            if (taskSlot.IsFunctioning())
            {
                building.Assess(assessAmount);
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
