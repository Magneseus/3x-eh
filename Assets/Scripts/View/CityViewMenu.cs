using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CityViewMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject buildingPane;
    public GameObject cnTowerIcon;
    public GameObject dropdownPanel;
    public GameObject label;
    public static bool showBuildingPane = false;
    public static bool isCancel = false;

   // public static Vector3 settingPos;
   // public static Vector3 cnTowerIconPos;
   // public static Vector3 dropdownPanelPos;
    //public static Vector3 buildingPanelPos;

   // public static Vector3 bufferPos;

    //public Vector3 originalPosSettingMenu

    // Use this for initialization
    void Start () {
        menu.SetActive(false);
        buildingPane.SetActive(showBuildingPane);
        dropdownPanel.SetActive(false);
       // settingPos = menu.transform.position;
       // cnTowerIconPos = cnTowerIcon.transform.position;
       // dropdownPanelPos = dropdownPanel.transform.position;
       // buildingPanelPos = buildingPane.transform.position;
        //bufferPos = new Vector3(99999,99999,99999);
       // menu.transform.position = bufferPos;
        //cnTowerIcon.transform.position = bufferPos;
       // dropdownPanel.transform.position = bufferPos;
       // buildingPane.transform.position = bufferPos;

    }
	
	// Update is called once per frame
	void Update () {
        buildingPane.SetActive(showBuildingPane);
    }

    public void ShowMenu()
    {
        menu.SetActive(true);
        //menu.transform.position = settingPos;
    }

    public void HideMenu()
    {
        menu.SetActive(false);
        //menu.transform.position = bufferPos;
    }
    public void ShowBuilding()
    {
        showBuildingPane = true;
        //Debug.Log("showBuilding");
        //buildingPane.SetActive(true);
        //buildingPane.transform.position = buildingPanelPos;
    }

    public void HideBuilding()
    {
        showBuildingPane = false;
        isCancel = true;
        //buildingPane.SetActive(false);
        //buildingPane.transform.position = bufferPos;
    }

    public void HighLight()
    {
        cnTowerIcon.GetComponent<Image>().color = new Color(1.0f,1.0f,1.0f,1.0f);
    }

    public void HighLightOff()
    {
        //Debug.Log("Highlight off");
        cnTowerIcon.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.7f);
    }

    public void CallDropdown()  
    {
        if (!dropdownPanel.activeSelf)
        {
            dropdownPanel.SetActive(true);
            //dropdownPanel.transform.position = dropdownPanelPos;
        }
        else if (dropdownPanel.activeSelf)
        {
            dropdownPanel.SetActive(false);
            //dropdownPanel.transform.position = bufferPos;
        }
    }

    public void SetTask()
    {
        GameObject selectedTask = EventSystem.current.currentSelectedGameObject;
        if(selectedTask.transform.parent.name == "DropdownDown"&&selectedTask != null)
        {
            label.GetComponent<Text>().text = selectedTask.transform.GetChild(0).GetComponent<Text>().text;
            dropdownPanel.SetActive(false);
            //dropdownPanel.transform.position = bufferPos;
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Region");
    }
}
