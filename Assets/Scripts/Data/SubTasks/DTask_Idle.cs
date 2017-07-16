using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTask_Idle : DTask {



	public DTask_Idle(DBuilding dBuilding, string dName) : base(dBuilding, null, 5, dName, 1.0f)
	{
		
		ForceClean ();
		ForceFixed ();
	}

	public override void TurnUpdate(int numDaysPassed)
	{

		foreach (DTaskSlot taskSlot in slotList)
		{
			taskSlot.TurnUpdate(numDaysPassed);

	
		}
		maxPeople = numPeople + 1;
	}

	public override void AddPerson(DPerson dPerson)
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


}
