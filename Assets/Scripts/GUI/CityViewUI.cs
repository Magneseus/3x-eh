using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityViewUI : MonoBehaviour
{
    private GameController gameController;
    private Button endTurnButton;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // Set the end turn button's OnClick
        foreach (var button in GetComponentsInChildren<Button>())
        {
            if (button.name == "EndTurnButton")
            {
                endTurnButton = button;
            }
        }
        endTurnButton.onClick.AddListener(gameController.dGame.EndTurnUpdate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
