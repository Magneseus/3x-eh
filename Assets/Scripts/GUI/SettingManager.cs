using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {

    public GameObject settingPane;
    public GameObject loadingSceneManager;
    public GameObject confirmPane;
    public GameObject mainPane;
    public Image loadingImage;

    public GameObject saveHint;
    


    private bool isQuit = false;

    private GameController gameController;

	// Use this for initialization
	void Start () {

        mainPane.SetActive(false);
        settingPane.SetActive(false);
        confirmPane.SetActive(false);
        loadingImage.color = Color.clear;
        loadingImage.enabled = false;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isQuit)
        {
          //  StartCoroutine(LevelSwitch());
        }
	}

    public void ShowSettings()
    {
        settingPane.SetActive(true);
        mainPane.SetActive(true);
        gameController.dGame.GameState = DGame._gameState.MENU;
		GameObject.Find ("SfxLibrary").GetComponents<AudioSource>()[6].Play();
    }

    public void HideSettings()
    {
        settingPane.SetActive(false);
        gameController.dGame.GameState = DGame._gameState.PLAY;
    }

    public void ShowConfirm()
    {
        confirmPane.SetActive(true);
    }

    public void HideConfirm()
    {
        confirmPane.SetActive(false);
    }
    public void QuitGame()
    {
        isQuit = true;
        loadingImage.enabled = true;
    }
    public void HideMainPane()
    {
        mainPane.SetActive(false);
    }
    IEnumerator LevelSwitch()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(false, 1f);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenuSystem");
    }

    public void SwitchToMain()
    {
        // Save the "continue" game
        gameController.SaveGame(ContinueGame.continueFileName);

        // Destroy the buildings
        gameController.destroyCityAndBuildings();
        gameController.dGame.Reset();
        MainMenuManager mainMenuManager = GameObject.Find("MainMenuSystem").transform.Find("MainMenuObject").Find("MainMenuManager")
             .gameObject.GetComponent<MainMenuManager>();
        mainMenuManager.SwitchToMainMenu();
        if(GameObject.Find("BGMAudio")!=null)
         GameObject.Find("BGMAudio").SetActive(false);
        GameObject.Find("UI Canvas").SetActive(false);
    }

    public void SetSaveHint(bool isActive)
    {
        saveHint.SetActive(isActive);
    }
}
