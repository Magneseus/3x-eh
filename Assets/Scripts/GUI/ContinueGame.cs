using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour {

    private GameController gameController;
    private Button continueButton;
    public string continueFileName;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        continueButton = GetComponent<Button>();

        CheckIfContinueExists();
	}

    private void Awake()
    {
        CheckIfContinueExists();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Continue()
    {
        gameController.LoadGame(continueFileName);
    }

    private void CheckIfContinueExists()
    {
        if (gameController != null && continueButton != null)
        {
            if (gameController.listSavedGames().Contains(continueFileName))
            {
                continueButton.interactable = true;
            }
            else
            {
                continueButton.interactable = false;
            }
        }
    }
}
