using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePanel : MonoBehaviour {
    public GameObject PanelMeeplePrefab;

    private GameController gameController;
    private DTask_Idle idleTask;
    private List<GameObject> meeples = new List<GameObject>();
	// Use this for initialization
	void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
       /* if (gameController.dGame.currentCity.townHall.getIdleTask() != null)
        {
            idleTask = gameController.dGame.currentCity.townHall.getIdleTask();
            idleTask.SidePanel = this;
        }*/
		
	}
    public void GenerateMeeples()
    {
        if (meeples.Count > 0)
            DeleteMeeples();
        if(idleTask.NumPeople > 0)
        {
            for(int i=0; i< idleTask.NumPeople; i++)
            {
                GameObject go = Instantiate(PanelMeeplePrefab, this.transform);
                go.GetComponent<MeepleController>().dPerson = idleTask.SlotList[i].Person;
                meeples.Add(go);
            }
        }
    }
    public void DeleteMeeples()
    {
        foreach(GameObject go in meeples)
        {
            Destroy(go);
        }
    }
}
