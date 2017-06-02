using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityViewMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject buildingPane;

	// Use this for initialization
	void Start () {
        menu.SetActive(false);
        buildingPane.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowMenu()
    {
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }
    public void ShowBuilding()
    {
        buildingPane.SetActive(true);
    }

    public void HideBuilding()
    {
        buildingPane.SetActive(false);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Region");
    }
}
