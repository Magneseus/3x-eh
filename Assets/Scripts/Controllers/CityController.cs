using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public DCity dCity;    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    internal void ConnectToDataEngine(DGame dGame, string cityName)
    {
        dCity = new DCity(cityName, this);
        dGame.Cities.Add(cityName, dCity);
    }
}
