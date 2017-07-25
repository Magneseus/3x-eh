using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {


    public Text[] resources = new Text[6];
	public List<string> resNames = new List<string>();
 

	private GameController gameController;
	private DCity dCity;


    // Use this for initialization
    void Start () {
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		dCity = null;
	}
	
	// Update is called once per frame
	void Update () {


		dCity = gameController.dGame.currentCity;


		if(dCity != null) {
		//	resources[0].text = "Population: " + dCity.People.Count;
			resources[1].text = "Food: " + dCity.GetResource("Food").Amount;
			resources[2].text = "Fuel: " + dCity.GetResource("Fuel").Amount;
			resources[3].text = "Materials: " + dCity.GetResource("Materials").Amount;
			resources[4].text = "Medicine: " + dCity.GetResource("Medicine").Amount;
			resources[5].text = "Shelter: " + dCity.GetResource("Shelter").Amount + " (-" + dCity.ShelterConsumedPerTurn() + ")";



			//Find resoruce names from engine nad place in resNames

		/*	resources[0].text = resNames[0]  + " : " + dCity.GetResource(resNames[0]);

			resources[1].text = resNames[1] + " : " + dCity.GetResource(resNames[1]);

			resources[2].text = resNames[2] + " : " + dCity.GetResource(resNames[2]);

			resources[3].text = resNames[3] + " : " + dCity.GetResource(resNames[3]);

			resources[4].text = resNames[4] + " : " + dCity.GetResource(resNames[4]);

			resources[5].text = resNames[5] + " : " + dCity.GetResource(resNames[5]);*/

		}
    }

}
