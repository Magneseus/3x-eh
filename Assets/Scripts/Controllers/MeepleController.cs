using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleController : MonoBehaviour {

    public DPerson dPerson;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void ConnectToDataEngine(DGame dGame, string cityName)
    {
        dPerson= new DPerson(dGame.Cities[cityName], this);
    }
}
