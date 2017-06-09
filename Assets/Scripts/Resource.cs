using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : TurnUpdatable
{
    public int    ID;
    public string Name;
    public float  Value;
    
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }
}
