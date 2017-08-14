using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour {

    private GameController gameController;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Continue()
    {
        gameController.LoadGame("Continue.json");
    }
}
