﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTask_Idle : DTask {



	public DTask_Idle(DBuilding dBuilding, string dName) : base(dBuilding, null, 1, dName, 1.0f)
	{
		
		ForceClean ();
		ForceFixed ();
		Reszie();
	}

	public override void TurnUpdate(int numDaysPassed)
	{

		foreach (DTaskSlot taskSlot in slotList)
		{
			taskSlot.TurnUpdate(numDaysPassed);
				
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
					if(dPerson.Task != null)
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
		if(numPeople == maxPeople)
			AddSlot();
		if(maxPeople - numPeople >= 2)
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
		if(slotList[slotList.Count - 1].Person == null)
		{
			slotList.RemoveAt(slotList.Count - 1);
		}
	}
}