using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

  private int storageCapacity;
  private int currentStorageCount;
  private int civilianCapacity;
  private int currentCivilianCount;


	// Use this for initialization
	void Start ()
  {

	}

	// Update is called once per frame
	void Update ()
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
