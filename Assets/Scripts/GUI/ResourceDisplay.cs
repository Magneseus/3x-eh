using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour {
    public Text Population;
    public Text Food;
    public Text Fuel;
    public Text Materials;
    public Text Medicine;
    public Text date;
    public Text Shelter;

    private DCity dCity;
    private GameController gameController;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        dCity = null;
	}

	// Update is called once per frame
	void Update () {
        date.text = DateSeasonString();
        dCity = gameController.dGame.currentCity;

        if (dCity != null)
        {
            Population.text = "Population: " + dCity.People.Count;
            Food.text = "Food: " + dCity.GetResource("Food").Amount;
            Fuel.text = "Fuel: " + dCity.GetResource("Fuel").Amount;
            Materials.text = "Materials: " + dCity.GetResource("Materials").Amount;
            Medicine.text = "Medicine: " + dCity.GetResource("Medicine").Amount;
            Shelter.text = "Shelter: " + dCity.GetResource("Shelter").Amount + " (-" + dCity.ShelterConsumedPerTurn() + ")";
        }
    }

    public string DateSeasonString()
    {
        string result = gameController.dGame.currentDateString;
        result += "   " + Constants.SEASON_DISPLAY_NAMES[(int)gameController.dGame.currentCity.Season];
        return result;
    }
}
