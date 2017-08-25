using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public Text[] resources = new Text[6];
    public string[] resNames = new string[6];
    public Text date;
    private DCity dCity;
    private GameController gameController;

    // Use this for initialization
    void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dCity = null;

    }
	
	// Update is called once per frame
	void Update () {

        dCity = gameController.dGame.currentCity;
        date.text = DateSeasonString();

        if (dCity == null) return;
        resources[0].text = resNames[0] + " : " + dCity.People.Count;

        resources[1].text = resNames[1] + " : " + dCity.GetResource(resNames[1]).Amount;

        resources[2].text = resNames[2] + " : " + dCity.GetResource(resNames[2]).Amount;

        resources[3].text = resNames[3] + " : " + dCity.GetResource(resNames[3]).Amount;

        resources[4].text = resNames[4] + " : " + dCity.GetResource(resNames[4]).Amount;

        resources[5].text = resNames[5] + " : " + dCity.GetResource(resNames[5]).Amount +"( "+dCity.ShelterConsumedPerTurn()+" )";

    }

    public string DateSeasonString()
    {
        string result = gameController.dGame.currentDateString;
        result += "   " + Constants.SEASON_DISPLAY_NAMES[(int)gameController.dGame.currentCity.Season];
        return result;
    }
}
