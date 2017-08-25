using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {

    public DCity dCity;
    public Sprite sprite;
    public GameController gameController;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void assignGameController(GameController game)
    {
      this.gameController = game;
    }

    internal void ConnectToDataEngine(DGame dGame, string cityName)
    {
        dCity = new DCity(cityName, this, dGame.DefaultSeasonStartDates, dGame.CurrentDate);
        dGame.Cities.Add(cityName, dCity);

        Sprite sp = UpdateSprite();

        //resize
        GetComponent<SpriteRenderer>().transform.localScale = new Vector3(Screen.width / (sp.bounds.size.x * 100), Screen.height / (sp.bounds.size.y * 100), 1);
    }

    public void ConnectToDataEngine(DGame dGame, DCity dCity)
    {
        this.dCity = dCity;

        Sprite sp = UpdateSprite();

        //resize
        GetComponent<SpriteRenderer>().transform.localScale = new Vector3(Screen.width / (sp.bounds.size.x * 100), Screen.height / (sp.bounds.size.y * 100), 1);
    }

    public Sprite UpdateSprite()
    {
        string cityName = dCity.Name;
        string seasonSuffix = "_" + Constants.SEASON_DISPLAY_NAMES[(int)dCity.Season].ToLower();
        Sprite sp = Resources.Load<Sprite>(Constants.CITY_SPRITE_PATH + cityName + seasonSuffix);
        GetComponent<SpriteRenderer>().sprite = sp;
        return sp;
    }
}
