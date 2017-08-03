using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using SimpleJSON;

public class DTask_Assess : DTask
{
    private float assessAmount;
    private float oAssessAmount;
    private float falloff;

    public DTask_Assess(DBuilding dBuilding, float assessAmount, int dMaxPeople, string dName) : base(dBuilding, null, dMaxPeople, dName, 0.0f)
    {
        this.assessAmount = assessAmount;
        oAssessAmount = assessAmount;
        falloff = .9f;

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

                assessAmount = Mathf.Max(assessAmount * falloff, oAssessAmount / 16);
            }
        }
    }

    public override JSONNode SaveToJSON()
    {
        JSONNode returnNode = base.SaveToJSON();

        returnNode.Add("specialTask", new JSONString("assess"));
        returnNode.Add("assessAmount", new JSONNumber(assessAmount));

        return returnNode;
    }

    #region Accessors

    public float AssessAmount
    {
        get { return assessAmount; }
        set { assessAmount = Mathf.Clamp01(value); }
    }

    #endregion
}
