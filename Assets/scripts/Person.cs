using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour, TurnUpdatable
{
    int age;
    float happiness;
    float thirst;
    float hunger;


	// Use this for initialization
	void Start () {
		
	}

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }
}
