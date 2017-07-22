using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public Text time;
    public GameObject resourcesManager;


    private int year = 2017;
    private int month = 8;
    private int increment = 1;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        /*if (month == 1)
        {
            time.text = " Jan., " + year;
        }else if (month == 2)
        {
            time.text = " Feb., " + year;
        }
        else if (month == 3)
        {
            time.text = " Mar., " + year;
        }
        else if (month == 4)
        {
            time.text = " Apr., " + year;
        }
        else if (month == 5)
        {
            time.text = " May, " + year;
        }
        else if (month == 6)
        {
            time.text = " Jun., " + year;
        }
        else if (month == 7)
        {
            time.text = " Jul., " + year;
        }
        else if (month == 8)
        {
            time.text = " Aug., " + year;
        }
        else if (month == 9)
        {
            time.text = " Sept., " + year;
        }
        else if (month == 10)
        {
            time.text = " Oct., " + year;
        }
        else if (month == 11)
        {
            time.text = " Nov., "+ year;
        }
        else if (month == 12)
        {
            time.text =  " Dec., "+ year;
        }
        else
        {
            time.text = "Error Time";
        }*/
       time.text = year + " - " + month.ToString("00");
    }

    public void IncrementTime()
    {
        if(month + increment <= 12)
        {
            month += increment;
        }
        else
        {
            month = month + increment - 12;
            year++;
        }
        increment = 1;
    }

    /*public void IncrementResources()
    {
        for(int i = 0; i < resourcesManager.GetComponent<ResourceManager>().res.Length; i++)
            resourcesManager.GetComponent<ResourceManager>().res[i] += (TaskSlotDetector.resIncrement[i]*increment);
        Debug.Log("res1 "+resourcesManager.GetComponent<ResourceManager>().res[0]+": incres "+ TaskSlotDetector.resIncrement[0] * increment);
    }*/

    public void SetIncrement(int increment)
    {
        this.increment = increment;
    }
}
