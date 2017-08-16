using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomUIManager : MonoBehaviour {

    public Text eventName;
    public Text eventContent;
    public GameObject eventPane;
    private GameController gameController;
    private DGame dGame;
    
	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
       
        dGame = gameController.dGame;
        eventPane.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (dGame.currentEvent != null)
        {
            //get event name to event name Text and event
            eventPane.SetActive(true);
            eventContent.text = dGame.currentEvent.promptText;

        }
	}


    public void SetOption(int option)
    {        
        dGame.ResolveEvent(option);
        dGame.currentEvent = null;
        eventPane.SetActive(false);
        Debug.Log("close window"+eventPane.activeSelf);
    }
}
