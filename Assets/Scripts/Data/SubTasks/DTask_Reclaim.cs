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
        // NOT IMPLEMENTED YET


        //if (listOfPeople.Count > 0)
        //{
        //    TODO: Make this into a exponential scale or something
        //     for (int i = 0; i < listOfPeople.Count; ++i)
        //        foreach (var task in building.tasks)
        //        {
        //            if (task.Value.IsReclaimed())
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                task.Value.Repair(reclaimAmount);
        //                if (building.tasks.GetLast().equals(task) && task.Value.IsReclaimed())
        //                    DisableTask();
        //                break;
        //            }
        //        }
        //}

        //Check if we've fully reclaimed the building, and if so disable task
        //if (building.LevelReclaimed >= Constants.BUILDING_MAX_RECLAIM)
        //{
        //    DisableTask();
        //}
    }

    #region Accessors

    public float ReclaimAmount
    {
        get { return reclaimAmount; }
        set { reclaimAmount = Mathf.Clamp01(value); }
    }

    #endregion
}
