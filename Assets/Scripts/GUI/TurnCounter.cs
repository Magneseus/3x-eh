using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCounter : MonoBehaviour
{
    public Text turnCounterText;
    private DGame gameController;

	// Use this for initialization
	void Start ()
    {
        turnCounterText = GetComponent<Text>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().dGame;
	}
	
	// Update is called once per frame
	void Update ()
    {
        turnCounterText.text = (gameController.TurnNumber).ToString();
	}
}
