using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityViewUI : MonoBehaviour
{
    private GameController gameController;
    private Button endTurnButton;

	// Use this for initialization
	void Start ()
    {
        // Resize to the Screen
        ResizeToScreen();

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // Set the end turn button's OnClick
        foreach (var button in GetComponentsInChildren<Button>())
        {
            if (button.name == "NextTurnButton")
            {
                endTurnButton = button;
            }
        }
		endTurnButton.onClick.AddListener(gameController.EndTurnButtonCallback);
	}

    void Awake()
    {
        ResizeToScreen();
    }
	
	public void ResizeToScreen()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
    }
}
