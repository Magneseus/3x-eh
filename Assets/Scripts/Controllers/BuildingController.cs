using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingController : MonoBehaviour {//, IPointerEnterHandler, IPointerExitHandler {

    public DBuilding dBuilding;
    private List<TaskController> taskControllers;

    private float taskControllerVisibilityTimeout = 0.2f;
    private bool taskControllerVisible;
    private int MouseOverCount = 0;

	// Use this for initialization
	void Start () {
        if (taskControllers == null)
        {
            taskControllers = new List<TaskController>();
        }

        SetTaskControllerVisibility(false);
    }

	// Update is called once per frame
	void Update () {

	}

    #region MouseOver Functions

    public void OnMouseEnter()
    {
        IncreaseMouseOverCount();
    }

    public void OnMouseExit()
    {
        DecreaseMouseOverCount();
    }

    public void IncreaseMouseOverCount()
    {
        MouseOverCount++;

        if (MouseOverCount == 1)
        {


            //Relating to building info text on hover
            Vector3 textPos =  this.transform.position;
            textPos.x += 0.5f;
            transform.Find("BuildingInfo").transform.position = textPos;
            transform.Find("BuildingInfo").GetComponent<TextMesh>().text = string.Format("Status: {0}\nAssessed: {1}\nDamaged: {2}\nInfected: {3}", dBuilding.StatusAsString, dBuilding.LevelAssessed, dBuilding.LevelDamaged , dBuilding.LevelInfected);

            SetTaskControllerVisibility(true);
        }
    }

    public void DecreaseMouseOverCount()
    {
        MouseOverCount--;

        if (MouseOverCount == 0)
        {
          // deletes building info text
          transform.Find("BuildingInfo").GetComponent<TextMesh>().text = "";
            StartCoroutine("MouseOffBuildingTimer");
        }
    }

    IEnumerator MouseOffBuildingTimer()
    {
        // Wait a half second
        yield return new WaitForSeconds(taskControllerVisibilityTimeout);

        // Continuously check MouseOverCount until it's 0 for sure
        while (MouseOverCount > 0)
            yield return new WaitForSeconds(taskControllerVisibilityTimeout);

        // If it's 0, then get rid of the task trays
        SetTaskControllerVisibility(false);
    }

    #endregion

    #region TaskController Functions
    public void SetTaskControllerVisibility(bool visibility)
    {
        taskControllerVisible = visibility;

        foreach (TaskController tc in taskControllers)
        {
            if(visibility)
                tc.UpdateSprite();
            tc.gameObject.SetActive(visibility);
        }
    }

    public void AddTaskController(TaskController taskController)
    {
        if (taskControllers == null)
        {
            taskControllers = new List<TaskController>();
        }

        // Add to the list
        taskControllers.Add(taskController);

        // Shift it down slightly
        Vector3 taskControllerPos = taskController.transform.position;
        taskControllerPos.y -= TaskTraySingle.WIDTH_CONST * taskControllers.Count;
        taskController.transform.position = taskControllerPos;

        // Set it to the current activity
        taskController.enabled = taskControllerVisible;
    }

    public void RemoveTaskController(TaskController taskController)
    {
        //TODO: Reorganize existing task controllers (shouldn't ever be used though...)
        taskControllers.Remove(taskController);
    }
    #endregion

    internal void ConnectToDataEngine(DGame dGame, string cityName, string buildingName)
    {
        dBuilding = new DBuilding(dGame.Cities[cityName], buildingName, this);
    }

}
