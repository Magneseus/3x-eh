using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, TurnUpdatable {

    public List<Building> ListOfBuildings;
    public List<Resource> ListOfResources;
    public List<Person> ListOfPopulation;

    string cityName;
    int civilianCount;
    

	// Use this for initialization
	void Start () {
		
	}

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Here we're probably just going to call TurnUpdate on all the
        // buildings/people/resources
        foreach (Building b in ListOfBuildings)
        {
            b.TurnUpdate(numDaysPassed);
        }
        foreach (Resource r in ListOfResources)
        {
            r.TurnUpdate(numDaysPassed);
        }
        foreach (Person p in ListOfPopulation)
        {
            p.TurnUpdate(numDaysPassed);
        }
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
