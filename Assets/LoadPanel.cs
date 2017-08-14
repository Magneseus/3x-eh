using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {

    public GameObject LoadObjectPrefab;

    private GameController gameController;
    public GameObject menuManager;
    private List<GameObject> loadButtons;
    private string fileNameToLoad;

    // Use this for initialization
    void Start () {
        fileNameToLoad = null;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        loadButtons = new List<GameObject>();
        displayFiles(gameController.listSavedGames());
	}
	
	// Update is called once per frame
	void Update () {
    }
    public void displayFiles(List<string> fileNames)
    {
        foreach(GameObject go in loadButtons)
        {
            Destroy(go);
        }
        foreach(string s in fileNames)
        {
           GameObject go = Instantiate(LoadObjectPrefab, this.transform) as GameObject;
           Text t = go.GetComponentInChildren<Text>();
           t.text = s;
            Button b = go.GetComponentInChildren<Button>();
            b.onClick.AddListener(delegate{ setFileName(s); });
            loadButtons.Add(go);
        }
    }
    void setFileName(string s)
    {
        fileNameToLoad = s;
    }
    public void load()
    {
        if (fileNameToLoad != null)
            if (menuManager != null)
                menuManager.GetComponent<MainMenuManager>().LoadGame(fileNameToLoad + ".json");
            else
                gameController.LoadGame(fileNameToLoad + ".json");
    }
}
