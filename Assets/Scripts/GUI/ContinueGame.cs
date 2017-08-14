using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueGame : MonoBehaviour {

    private GameController gameController;
    private Button continueButton;
    public GameObject menuManager;
    public static string continueFileName = "__Continue.json";

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
        if (menuManager != null)
            menuManager.GetComponent<MainMenuManager>().LoadGame(continueFileName);
    }

    private void CheckIfContinueExists()
    {
        if (gameController != null && continueButton != null)
        {
            if (gameController.listSavedGames().Contains(continueFileName.Substring(0, continueFileName.Length-5)))
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
