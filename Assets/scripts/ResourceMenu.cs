using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMenu : MonoBehaviour {

    public GameObject ResourceMan;
    public Text re1;
    public Text re2;
    public Text re3;
    public Text re4;
    public Text re5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        re1.text = "Food: " + ResourceMan.GetComponent<ResourceManager>().GetFood();
        re2.text = "Fuel: " + ResourceMan.GetComponent<ResourceManager>().GetFuel();
        re3.text = "Edu: " + ResourceMan.GetComponent<ResourceManager>().GetEdu();
        re4.text = "Happiness: " + ResourceMan.GetComponent<ResourceManager>().GetHP();
        re5.text = "Population: " + ResourceMan.GetComponent<ResourceManager>().GetPop();
    }
}
