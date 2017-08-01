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
    private Image cityIcon;

    private bool isEnabled;

	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cityNameText = GetComponentInChildren<Text>();
        cityIcon = GetComponent<Image>();

        cityNameText.enabled = false;
        isEnabled = true;
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

    public bool Enabled
    {
        get { return isEnabled; }
        set
        {
            isEnabled = value;

            if (isEnabled)
            {
                cityIcon.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                cityIcon.color = new Color(1f, 1f, 1f, 0.33f);
            }
        }
    }

    #endregion

    #region EventHandlers
    public void OnPointerClick(PointerEventData eventData)
    {
        // We have selected this city, tell game manager
        if (isEnabled)
        {
            gameController.SelectCity(this.cityName);
            cityNameText.enabled = false;
        }
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
