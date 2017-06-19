using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepleTray : MonoBehaviour {

  public int Capacity = 10;
  // public List<GameObject> meeple = new List<GameObject>(10);
  public bool full = false;
  public GameObject slot;
  // public transform temp;
  public int Count = 0;

	// Use this for initialization
	void Start ()
  {
    if(slot == null)
    slot = Instantiate(Resources.Load("buildingSlot")) as GameObject;

    for(int i =0; i < Capacity; ++i)
    {
      Vector3 pos = new Vector3(this.transform.position.x + (i * slot.GetComponent<Renderer>().bounds.size.x ) ,(this.transform.position.y ),0);
       GameObject clone;
       clone = Instantiate(slot,pos,Quaternion.identity);
       clone.transform.parent  = this.transform;
    }
	}

	// Update is called once per frame
	void Update ()
  {

	}



}
