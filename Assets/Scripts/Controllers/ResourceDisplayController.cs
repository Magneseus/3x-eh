using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplayController : MonoBehaviour {
    public GameObject GameManagerController;
    public Text Population;
    public Text Food;
    public Text Fuel;
    public Text Materials;
    public Text Medecine;

    private DCity dCity;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
        dCity = GameManagerController.GetComponent<GameController>().dGame.Cities["Ottawa"];
        Population.text = "Population: " + dCity.People.Count;
        Food.text = "Food: " + dCity.GetResource("Food").Amount;
        Fuel.text = "Fuel: " + dCity.GetResource("Fuel").Amount;
        Materials.text = "Materials: " + dCity.GetResource("Materials").Amount;
        Medecine.text = "Medecine: " + dCity.GetResource("Medecine").Amount;
    }
}
