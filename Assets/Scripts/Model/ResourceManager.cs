using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    private int food = 10;
    private int fuel = 10;
    private int edu = 10;
    private int happiness = 10;
    private int population = 100;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetFood(int food)
    {
        this.food = food;
    }

    public void SetFuel(int fuel)
    {
        this.fuel = fuel;
    }

    public void SetEdu(int edu)
    {
        this.edu = edu;
    }

    public void SetHp(int happiness)
    {
        this.happiness = happiness;
    }

    public void SetPop(int population)
    {
        this.population = population;
    }

    public int GetFood()
    {
        return food;
    }

    public int GetFuel()
    {
        return fuel;
    }

    public int GetEdu()
    {
        return edu;
    }

    public int GetHP()
    {
        return happiness;
    }

    public int GetPop()
    {
        return population;
    }
}
