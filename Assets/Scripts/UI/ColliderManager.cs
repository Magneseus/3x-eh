using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour {

    public GameObject meeple;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnTriggerStay(Collider other)
    {
       // Debug.Log("collided");
        if (DragMeeple.dragging == false) { 
        //Debug.Log("BuildingPane "+!BuildingPane.activeSelf);
            if (other.gameObject.tag == "LandMark") {
                // Debug.Log("collided222");
                    CityViewMenu.showBuildingPane = true;
                meeple.SetActive(false);
                //BuildingPane.transform.position = CityViewMenu.buildingPanelPos;
            }
       }
    }

}
