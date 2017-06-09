using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : TurnUpdatable
{

    List<Building> listOfBuildings = new List<Building>();
    List<Resource> listOfResources = new List<Resource>();
    List<Person> listOfPopulation = new List<Person>();

    string cityName = "NoCityName";
    int cityAge = 0;

    public City()
    {

    }



    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Here we're probably just going to call TurnUpdate on all the
        // buildings/people/resources
        foreach (Building b in listOfBuildings)
        {
            b.TurnUpdate(numDaysPassed);
        }
        foreach (Resource r in listOfResources)
        {
            r.TurnUpdate(numDaysPassed);
        }
        foreach (Person p in listOfPopulation)
        {
            p.TurnUpdate(numDaysPassed);
        }

        cityAge += numDaysPassed;
    }



    public List<Building> Buildings
    {
        get { return listOfBuildings; }
    }

    public List<Resource> Resources
    {
        get { return listOfResources; }
    }

    public List<Person> Population
    {
        get { return listOfPopulation; }
    }

    public string Name
    {
        get { return cityName; }
        set { cityName = value; }
    }

    public int CivilianCount
    {
        get { return listOfPopulation.Count; }
    }

    public int Age
    {
        get { return cityAge; }
    }

}