using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, TurnUpdatable {

    public List<Person> ListOfPersons;

    private int storageCapacity;
    private int currentStorageCount;
    private int civilianCapacity;
    private int currentCivilianCount;


	// Use this for initialization
	void Start ()
    {

	}

    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {

    }


    void setCivilianCapacity(int cap)
    {
        this.civilianCapacity = cap;
    }
    int getCivilianCapacity()
    {
        return civilianCapacity;
    }

    void setStorageCapacity(int cap)
    {
        this.storageCapacity = cap;
    }
    int getStorageCapacity()
    {
        return storageCapacity;
    }
}
