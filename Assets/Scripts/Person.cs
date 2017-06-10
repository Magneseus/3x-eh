using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : TurnUpdatable
{
    public enum PAge
    {
        Youth,
        Adult,
        Elder
    };

    public PAge  Age;
    public float Happiness;
    public float Hunger;
       
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }
}
