using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour {

    private GameController gameControl;
    public Text saveName;
    public Text placeholder;

	// Use this for initialization
	void Start () {
        gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        placeholder.text = "Save Data - " + GameController.SaveNum;
	}

    public void SaveGame()
    {
        if (saveName.text == null)
        {
            gameControl.SaveGame(placeholder.text);
        }
        else
        {
            gameControl.SaveGame(saveName.text + " - " + GameController.SaveNum);
        }

        GameController.SaveNum++;

    }
}
