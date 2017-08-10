using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {

    public GameObject LoadObjectPrefab;

    private GameController gameController;
    private MainMenuManager menuManager;
    private List<GameObject> loadButtons;
    private string fileNameToLoad;

    // Use this for initialization
    void Start () {
        fileNameToLoad = null;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        menuManager = GameObject.Find("MainMenuManager").GetComponent<MainMenuManager>();
        loadButtons = new List<GameObject>();
        displayFiles(gameController.listSavedGames());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void displayFiles(List<string> fileNames)
    {
        foreach(string s in fileNames)
        {
           GameObject go = Instantiate(LoadObjectPrefab, this.transform) as GameObject;
           Text t = go.GetComponentInChildren<Text>();
           t.text = s;
            Button b = go.GetComponentInChildren<Button>();
            b.onClick.AddListener(delegate{ setFileName(s); });
        }
    }
    void setFileName(string s)
    {
        fileNameToLoad = s;
    }
    public void load()
    {
        if (fileNameToLoad != null)
            menuManager.LoadGame(fileNameToLoad+".json");
    }
}
