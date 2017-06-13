using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

  public MeepleSlot activeSlot;
  public Meeple activeMeeple;
  public GameObject meeple ;
  // public _BuildingUI activeBuilding;
	// Use this for initialization
	void Start ()
  {
     //loading resource for the meeple sprite (this breaks if not assigned to in unity editor for some reason [must be a way to do this programmatically])
    if (meeple == null)
      meeple = Instantiate(Resources.Load("meeplePrefab")) as GameObject;

    GameObject _startTray = GameObject.Find("startTray") as GameObject;
    MeepleTray startTray = _startTray.GetComponent<MeepleTray>() as MeepleTray;
    MeepleSlot[] meeples = startTray.GetComponentsInChildren<MeepleSlot>() as MeepleSlot[];

    foreach (MeepleSlot m1 in meeples) //assignment to 'startTray'
    {
      GameObject _meeple = Instantiate(meeple);
      Meeple thisMeeple = _meeple.GetComponent<Meeple>() as Meeple;
      m1.addMeeple(thisMeeple);
    }
	}

	// Update is called once per frame
	void Update ()
  {

	}
}
