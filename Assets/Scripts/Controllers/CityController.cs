using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public DCity dCity;
    public Sprite sprite;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    internal void ConnectToDataEngine(DGame dGame, string cityName)
    {
      GetComponent<SpriteRenderer>().sprite =
      Resources.Load(Constants.CITY_SPRITE_PATH + cityName) as Sprite;
        dCity = new DCity(cityName, this);
        dGame.Cities.Add(cityName, dCity);
    }
}
