using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TurnUpdatable {

    public List<Resource> ResourceInputPerTurn;
    public List<Resource> ResourceOutputPerTurn;
    public List<Person> ListOfPersons;

    // Don't think we need most of these except for civilCap
    private int storageCapacity;
    private int currentStorageCount;
    private int civilianCapacity;
    private int currentCivilianCount;    

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
