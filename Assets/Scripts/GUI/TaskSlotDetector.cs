using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaskSlotDetector : MonoBehaviour , IDropHandler{
    // add building names here to manually attatch slot name!!!!!
    //public GameObject resourceManager;
    public string[] slotNames = new string[7];
    public Text currBuilding;
    public Text showCapcity;

    public List<GameObject> meeples = new List<GameObject>();

    public int capcity = 3;

    public static int[] resIncrement = { 0,0,0,0,0,0};

    public GameObject item
    {
        get
        {
            if (transform.childCount > capcity)
                return transform.GetChild(0).gameObject;
            return null;
        }
        
    }

    void Start()
    {
        //Debug.Log("meeples size "+ TimeManager.meeples.Count);
    }

    void Update()
    {
        if (this.transform.name != slotNames[6])
        {
            showCapcity.text = "MaximuBuilding Capcity: " + capcity*100;
        }
        if(this.transform.name == slotNames[6])
        {
            showCapcity.text = "Maximum Meeple Capcity: " + capcity;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            MeepleControl.draggingItem.transform.SetParent(transform);
            MeepleControl.draggingItem.GetComponent<MeepleControl>().SetRealParentName(transform.name);
            //Debug.Log("transform name "+ this.transform.name);
			if(this.transform.name == slotNames[0])
            {
                resIncrement[0] += 10;
			}else if(this.transform.name == slotNames[1])
            {
                resIncrement[1] += 10;
			}else if(this.transform.name == slotNames[2])
            {
                resIncrement[2] += 10;
			}else if(this.transform.name == slotNames[3])
            {
                resIncrement[3] += 10;
			}else if(this.transform.name == slotNames[4])
            {
                resIncrement[4] += 10;
			}else if(this.transform.name == slotNames[5])
            {
                resIncrement[5] += 10;
			}else if(this.transform.name == slotNames[6])
            {//this slot is home for meeples

                
                if (MeepleControl.startParent.name == slotNames[0] && resIncrement[0] - 10 >= 0)
                {
                    resIncrement[0] -= 10;
                }else if(MeepleControl.startParent.name == slotNames[1] && resIncrement[1] - 10 >= 0)
                {
                    resIncrement[1] -= 10;
                }else if (MeepleControl.startParent.name == slotNames[2] && resIncrement[2] - 10 >= 0)
                {
                    resIncrement[2] -= 10;
                }else if (MeepleControl.startParent.name == slotNames[3] && resIncrement[3] - 10 >= 0)
                {
                    resIncrement[3] -= 10;
                }else if (MeepleControl.startParent.name == slotNames[4] && resIncrement[4] - 10 >= 0)
                {
                    resIncrement[4] -= 10;
                }else if (MeepleControl.startParent.name == slotNames[5] && resIncrement[5] - 10 >= 0)
                {
                    resIncrement[5] -= 10;
                }
                

            }
        }
    }
    
}
