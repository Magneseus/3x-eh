using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CityNode : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    private string cityName;
    private GameController gameController;
    private Text cityNameText;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cityNameText = GetComponentInChildren<Text>();
        cityNameText.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Accessors

    public string CityName
    {
        get { return cityName; }
        set
        {
            cityName = value;

            if (cityNameText == null)
                cityNameText = GetComponentInChildren<Text>();

            cityNameText.text = value;
        }
    }

    #endregion

    #region EventHandlers
    public void OnPointerClick(PointerEventData eventData)
    {
        // We have selected this city, tell game manager
        gameController.SelectCity(this.cityName);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        cityNameText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cityNameText.enabled = false;
    }
    #endregion
}
