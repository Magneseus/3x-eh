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
    


    private bool isQuit = false;

	// Use this for initialization
	void Start () {
        mainPane.SetActive(false);
        settingPane.SetActive(false);
        confirmPane.SetActive(false);
        loadingImage.color = Color.clear;
        loadingImage.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isQuit)
        {
            StartCoroutine(LevelSwitch());
        }
	}

    public void ShowSettings()
    {
        settingPane.SetActive(true);
        mainPane.SetActive(true);
    }

    public void HideSettings()
    {
        settingPane.SetActive(false);
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
        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(true, 1f);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("MainMenuSystem");
    }

}
