using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    string cityName;
    int civilianCount;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCityName(string name)
    {
        cityName = name;
    }
    public string getCityName()
    {
        return cityName;
    }
    public void setCivilianCount(int count)
    {
        civilianCount = count;
    }
    public int getCivilianCount()
    {
        return civilianCount;
    }
}
