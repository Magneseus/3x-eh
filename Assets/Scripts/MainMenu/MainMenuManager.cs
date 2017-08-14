using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

    public GameObject loadingSceneManager;
    public GameObject camControl;
    public Transform newGameMount;
    public Transform mainMenuMount;

    public Button newGameBtn;
    public Button loadBtn;
    public Button optBtn;
    public Button creditBtn;
    public Button quitBtn;

    public float distance = 10f;

    private bool switching;
    private float waitForSeconds;

    // Use this for initialization
    void Start () {
        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(false, 3f);
        newGameBtn.interactable = false;
        loadBtn.interactable = false;
        optBtn.interactable = false;
        creditBtn.interactable = false;
        quitBtn.interactable = false;
        switching = false;
        waitForSeconds = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.SqrMagnitude(camControl.transform.position - newGameMount.position) < distance)
        {
            switching = true;
            if (switching)
            {
                loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(true, 1f);
                waitForSeconds -= Time.deltaTime;
                if (waitForSeconds <= 0.0f)
                {
                    switching = false;
                    waitForSeconds = 5.0f;
                    SwitchToGame();
                    camControl.GetComponent<CamControl>().setMount(mainMenuMount);
                }
            }
        }
        if(Vector3.SqrMagnitude(camControl.transform.position - mainMenuMount.position) <= distance && (!newGameBtn.interactable && !loadBtn.interactable && !optBtn.interactable && !creditBtn.interactable && !quitBtn.interactable))
        {
            
            newGameBtn.interactable = true;
            loadBtn.interactable = true;
            optBtn.interactable = true;
            creditBtn.interactable = true;
            quitBtn.interactable = true;
        }
        if(Vector3.SqrMagnitude(camControl.transform.position - mainMenuMount.position) > distance && (newGameBtn.interactable && loadBtn.interactable && optBtn.interactable && creditBtn.interactable && quitBtn.interactable))
        {
            newGameBtn.interactable = false;
            loadBtn.interactable = false;
            optBtn.interactable = false;
            creditBtn.interactable = false;
            quitBtn.interactable = false;

        }
	}


    public void SwitchToGame()
    {
        GameObject.Find("Game").transform.Find("Main Camera").gameObject.SetActive(true);
        GameObject.Find("Game").transform.Find("UI Canvas").gameObject.SetActive(true);
        GameObject.Find("MainMenuSystem").transform.Find("MainMenuObject").gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void SwitchToMainMenu()
    {
        
        GameObject.Find("Game").transform.Find("Main Camera").gameObject.SetActive(false);
        GameObject.Find("MainMenuSystem").transform.Find("MainMenuObject").gameObject.SetActive(true);
        camControl.GetComponent<CamControl>().setMount(camControl.GetComponent<CamControl>().mainMenuMount);
        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(false, 3f);
        camControl.GetComponent<CamControl>().setMount(camControl.GetComponent<CamControl>().mainMenuMount);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void LoadGame(string s)
    {
        GameObject.Find("GameController").GetComponent<GameController>().LoadGame(s);
        GameObject.Find("Game").transform.Find("Main Camera").gameObject.SetActive(true);
        GameObject.Find("Game").transform.Find("UI Canvas").gameObject.SetActive(true);
        GameObject.Find("MainMenuSystem").transform.Find("MainMenuObject").gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
