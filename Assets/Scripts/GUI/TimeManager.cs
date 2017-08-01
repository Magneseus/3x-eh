using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

	public static bool startNextTurn = false;
    /*public static List<GameObject> meeples = new List<GameObject>();

	public GameObject resourcesManager;
	public GameObject meepleSlot;
	public GameObject meeple;
	public GameObject meepleText;
	public GameObject meepleImage;
	public GameObject meepleSlotParent;
    */
	public Image switchImage;
	public Text switchText;
	public GameObject loadingSceneManager;
	//public AudioSource bgm;
    
	//public GameObject ordinaryText;
	//public GameObject buildingPanel;
	//public GameObject inputPanel;
	//public int lastTurnPopulation;
	//public int currentTenDigitNumber;
	//public GameObject meepleTextIncrementer;
    //public int slotCounter = 2;
    //public int meepleCounter = 1;
	//public int groupCounter = 0;
	//public GameObject[] meepleSlots;

	//public List<GameObject> slotList;

	//private int year = 2017;
	//private int month = 8;
	//private int increment = 1;
	private bool isSwitchIn = false;
	private bool isSwitchOut = false;

	private bool functionLimiter;
    /*
	public static int randomEventCategory;
	public static int randomEventAmount;
	public static int randomEventDue;
	public static int randomEventFirstDue;

	public static List<int> randomEventSaver = new List<int> ();
	public static List<int> randomDueSaver = new List<int> ();

	public static int workingMeeple = 0;

	public GameObject eventScrollBar;
	public GameObject instructionMask;
	public GameObject eventTellerInTimeManager;

	public int randomNumberThisItem;
	public int randomNumberFirstItem;
    */
	// Use this for initialization
	void Start () {
		//meepleTextIncrementer = null;
		//meepleSlots = GameObject.FindGameObjectsWithTag ("Slot");
		switchImage.color = Color.clear;
		switchImage.enabled = false;
		switchText.color = new Color(255,255,255,0);
		switchText.enabled = false;
		functionLimiter = false;
        
        //meeples.Add(meeple);
	}

	// Update is called once per frame
	void FixedUpdate () {
        /*
		time.text = year + " - " + month.ToString("00");
		if (isSwitchIn) {
			switchText.color = Color.Lerp (switchText.color, Color.white, Time.deltaTime);
			StartCoroutine (SwitchIn ());
		}

		if (isSwitchOut) {
			switchText.color = Color.Lerp (switchText.color, new Color(255,255,255,0), Time.deltaTime);
			StartCoroutine (SwitchOut());
			functionLimiter = false;
		}*/
	}
    /*
	public void IncrementTime()
	{
		if(month + increment <= 12)
		{
			month += increment;
		}
		else
		{
			month = month + increment - 12;
			year++;
		}
		increment = 1;
	}
    */
	public void Switch(){
		//Debug.Log ("Switch is called");
		//bgm.volume = 0.2f;
		this.GetComponent<AudioSource> ().Play ();
		switchImage.enabled = true;
		switchText.enabled = true;
		isSwitchIn = true;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		startNextTurn = true;
	}
    /*
	public void IncrementResources()
	{
		
		lastTurnPopulation = resourcesManager.GetComponent<ResourceManager> ().res [5];

		for(int i = 0; i < resourcesManager.GetComponent<ResourceManager>().res.Length; i++)
        {
            if (resourcesManager.GetComponent<ResourceManager>().res[5] >= meepleSlot.GetComponent<TaskSlotDetector>().capcity*100)
            {
                TaskSlotDetector.resIncrement[5] = 0;
                
            }                
            resourcesManager.GetComponent<ResourceManager>().res[i] += (TaskSlotDetector.resIncrement[i] * increment);
        }

		currentTenDigitNumber = resourcesManager.GetComponent<ResourceManager> ().res [5] / 10 % 10;

		//Meeple Generator
		if (currentTenDigitNumber == 1 && meepleCounter <= meepleSlot.GetComponent<TaskSlotDetector>().capcity) {
            //for (int i = 0; i < (resourcesManager.GetComponent<ResourceManager> ().res [5] - lastTurnPopulation)/100; i++){
            //Debug.Log ("1 MEEPLE GENERATED!");
            //slotCounter += 1;
            meepleCounter++;
            //Debug.Log("meeple counter "+meepleCounter);
			//if (slotCounter % 10 == 0){
			//Debug.Log ("wow");
			//}
			/*Debug.Log (slotCounter);
			GameObject meepleSlotRT = GameObject.Instantiate (meepleSlot, meepleSlot.transform.position, meepleSlot.transform.rotation) as GameObject;
			meepleSlotRT.transform.SetParent (meepleSlotParent.transform, true);
			meepleSlotRT.transform.localScale = meepleSlot.transform.localScale;
			GameObject meepleRT = GameObject.Instantiate (meeple, meeple.transform.position, meeple.transform.rotation) as GameObject;
			meepleRT.transform.SetParent (meepleSlot.transform, true);
            meepleRT.GetComponent<MeepleControl>().SetRealParentName(meepleSlot.transform.name);
			meepleRT.transform.localScale = meeple.transform.localScale;
			//meepleRT.transform.GetChild (0).GetComponent<Text> ().text = "10 People";
			meepleTextIncrementer = meepleRT.transform.GetChild (0).gameObject;

			//these 2 lines will fix the red cross bug in scene view under high resolution
			meepleRT.GetComponent<RectTransform> ().offsetMax = new Vector2 (0f, 0f);
			meepleRT.GetComponent<RectTransform> ().offsetMin = new Vector2 (0f, 0f);
            meeples.Add(meepleRT);
            meepleRT.SetActive(true);
		}
        /*
		if (meepleTextIncrementer != null){
			if (currentTenDigitNumber == 0){
				meepleTextIncrementer.GetComponent<Text>().text = "100 People";
			}
			else{
				meepleTextIncrementer.GetComponent<Text>().text = currentTenDigitNumber.ToString() + "0 People";
			}
		}

		RandomEventGenerator ();

	}

	public void RandomEventGenerator(){
		//Generate Random Event
		randomEventSaver.Clear ();
		randomDueSaver.Clear ();
		randomEventAmount = Random.Range (1,7);
		randomNumberFirstItem = Random.Range (1,21);
		randomEventFirstDue = Random.Range (3,20);
		randomEventSaver.Add(randomNumberFirstItem);
		randomDueSaver.Add (randomEventDue);

		for (int i=0; i<randomEventAmount;i++){
			randomNumberThisItem = Random.Range (1,21);
			randomEventSaver.Add (randomNumberThisItem);
		}


		RandomEventLibrary.RandomEventControl ();
		EventScrollBarIncrease ();

		foreach (Transform child in instructionMask.transform) {
			if (child.GetChild (0).gameObject.GetComponent<Text> ().text != "Tutorial") {
				child.GetChild (1).gameObject.GetComponent<Text> ().text = ((int.Parse (child.GetChild (1).gameObject.GetComponent<Text> ().text)) - 1).ToString ();
				if ((int.Parse (child.GetChild (1).gameObject.GetComponent<Text> ().text)) <= 0){
					Destroy (child.gameObject);
					eventTellerInTimeManager.SetActive(false);
				}
			}
		}
		if (TaskPaneManager.eventDueRT != null){
			TaskPaneManager.eventDueRT = ((int.Parse (TaskPaneManager.eventDueRT)) - 1).ToString ();
		}
				
	}

	public void SetIncrement(int increment)
	{
		this.increment = increment;
	}*/

	IEnumerator SwitchIn(){
		//Debug.Log ("SwitchIn is called");
		loadingSceneManager.GetComponent<LoadingSceneManager> ().Fade (true, 1f);
		//Debug.Log ("123456");

		yield return new WaitForSeconds(5f);

		isSwitchIn = false;
		isSwitchOut = true;
		//Debug.Log ("123");
	}

	IEnumerator SwitchOut(){
		//Debug.Log ("SwitchOut is called");
		loadingSceneManager.GetComponent<LoadingSceneManager> ().Fade (false, 1f);

		yield return new WaitForSeconds(5f);

		if (functionLimiter == false){
			ResetSwitch ();
		}


	}
   
	private void ResetSwitch(){
		//Debug.Log ("ResetSwitch is called");
		isSwitchOut = false;
		switchImage.enabled = false;
		switchText.enabled = false;
		functionLimiter = true;
		//bgm.volume = 1f;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		startNextTurn = false;
    }
    /*
	public void EventScrollBarIncrease(){
		eventScrollBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (120, 50 * eventScrollBar.transform.childCount);
	}*/


}
