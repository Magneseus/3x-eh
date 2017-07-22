using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour {

    public Text[] resources = new Text[6];
    public string[] resNames = new string[6];
    
    public int[] res = { 200, 500, 300, 800, 1000, 200 };

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        resources[0].text = resNames[0] + " : " + res[0];

        resources[1].text = resNames[1] + " : " + res[1];

        resources[2].text = resNames[2] + " : " + res[2];

        resources[3].text = resNames[3] + " : " + res[3];

        resources[4].text = resNames[4] + " : " + res[4];

        resources[5].text = resNames[5] + " : " + res[5];

    }

}
