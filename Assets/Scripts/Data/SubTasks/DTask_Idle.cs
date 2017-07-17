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
	}




}
