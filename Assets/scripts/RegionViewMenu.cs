using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionViewMenu : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadCountryLevel()
    {
        System.Console.Write("loading Country");
        SceneManager.LoadScene("Country");
    }

    public void LoadCity(string cityName)
    {
        System.Console.Write("loading City");
        SceneManager.LoadScene(cityName);
    }
}
