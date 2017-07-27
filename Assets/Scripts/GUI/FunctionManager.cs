using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionManager : MonoBehaviour {
    public GameObject functionPane;
   // public GameObject mainPane;

    private bool isActive = false;

	// Use this for initialization
	void Start () {
        functionPane.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isActive && !functionPane.activeSelf)
        {
            functionPane.SetActive(true);
        }
        if(!isActive && functionPane.activeSelf)
        {
            functionPane.SetActive(false);
        }
	}
/*
    public void OnMouseEnter()
    {
        functionPane.SetActive(true);
        mainPane.SetActive(false);
    }

    public void OnMouseExit()
    {
        functionPane.SetActive(false);
    }
*/
    public void SetIsActive(bool isActive)
    {
        this.isActive = isActive;
    }
}
