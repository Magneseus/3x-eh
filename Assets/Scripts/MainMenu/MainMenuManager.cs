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

	// Use this for initialization
	void Start () {
        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(false, 3f);
        newGameBtn.interactable = false;
        loadBtn.interactable = false;
        optBtn.interactable = false;
        creditBtn.interactable = false;
        quitBtn.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.SqrMagnitude(camControl.transform.position - newGameMount.position) < distance)
        {
            StartCoroutine(LoadingGame());
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

    IEnumerator LoadingGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loadingSceneManager.GetComponent<LoadingSceneManager>().Fade(true, 1f);
        yield return new WaitForSeconds(10f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
