using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DTask_Idle : DTask {

    private IdlePanel sidePanel;

    public DTask_Idle(DBuilding dBuilding, string dName) : base(dBuilding, null, 1, dName, 1.0f)
    {

        ForceClean();
        ForceFixed();
        Reszie();
    }

    public override void TurnUpdate(int numDaysPassed)
    {

        foreach (DTaskSlot taskSlot in slotList)
        {
            taskSlot.TurnUpdate(numDaysPassed);

        }
    }

    public override void RemovePerson(DPerson dPerson)
    {
        base.RemovePerson(dPerson);
        Reszie();
        if (sidePanel != null)
            sidePanel.GenerateMeeples();
    }
    public override void AddPerson(DPerson dPerson)
    {
        base.AddPerson(dPerson);
        Reszie();
          if (sidePanel != null)
            sidePanel.GenerateMeeples();
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
        maxPeople = numPeople + 1;
        DTaskSlot slot= new DTaskSlot(this);
        slotList.Add(slot);
        taskController.Resize();

        ForceClean();
		ForceFixed();
	}

	private void RemoveSlot()
	{
        maxPeople = numPeople + 1;

	    slotList.RemoveAt(slotList.Count - 1);
        taskController.Resize();

    }

    public override JSONNode SaveToJSON()
    {
        JSONNode returnNode = base.SaveToJSON();

        returnNode.Add("specialTask", new JSONString("idle"));

        return returnNode;
    }
    public IdlePanel SidePanel
    {
        get { return sidePanel; }
        set { sidePanel = value; }
    }
}
