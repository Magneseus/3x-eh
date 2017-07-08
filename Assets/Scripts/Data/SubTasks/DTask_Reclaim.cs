using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class DTask_Reclaim : DTask
{
    public const float DEFAULT_RECLAIM_AMOUNT = 0.1f;
    private float reclaimAmount;

    public DTask_Reclaim(DBuilding dBuilding, float reclaimAmount, int dMaxPeople, string dName) : base(dBuilding, null, dMaxPeople, dName)
    { }

    public DTask_Reclaim(DBuilding dBuilding) : this(dBuilding, DEFAULT_RECLAIM_AMOUNT, 4, "default_reclaim_task")
    { }

    public override void TurnUpdate(int numDaysPassed)
    {
        if (listOfPeople.Count > 0)
        {
            // TODO: Make this into a exponential scale or something
            for (int i = 0; i < listOfPeople.Count; ++i)
                building.Reclaim(0.1f);
        }

        // Check if we've fully reclaimed the building, and if so disable task
        if (building.IsReclaimed())
        {
            DisableTask();
        }
    }

    #region Accessors

    public float ReclaimAmount
    {
        get { return reclaimAmount; }
        set { reclaimAmount = Mathf.Clamp01(value); }
    }

    #endregion
}
