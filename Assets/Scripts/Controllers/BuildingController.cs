using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

    public DBuilding dBuilding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal void ConnectToDataEngine(DGame dGame, string cityName, string buildingName)
    {
        DBuilding dBuilding = new DBuilding(dGame.Cities[cityName], buildingName, this);        
    }
}
