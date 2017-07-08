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
    {}

    public DTask_Assess(DBuilding dBuilding) : this(dBuilding, DEFAULT_ASSESS_AMOUNT, 4, "default_assess_task")
    {}

    public override void TurnUpdate(int numDaysPassed)
    {
        if (listOfPeople.Count > 0)
        {
            // TODO: Make this into a exponential scale or something
            for (int i = 0; i < listOfPeople.Count; ++i)
                building.Assess(0.1f);
        }

        // Check if we've fully assessed the building, and if so disable task
        if (building.IsAssessed())
        {
            DisableTask();
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
