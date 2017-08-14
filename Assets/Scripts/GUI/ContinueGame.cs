using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour {

    private GameController gameContoller;
    public string contiuneFileName;

	// Use this for initialization
	void Start () {
        gameContoller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Continue()
    {
        gameContoller.LoadGame(contiuneFileName);
    }
}
