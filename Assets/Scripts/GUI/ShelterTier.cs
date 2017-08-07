using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShelterTier : MonoBehaviour
{
    private Text text;
    private Button tierRaiseButton;
    private Button tierLowerButton;
    private GameController gameController;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        text = GetComponent<Text>();
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            if (button.gameObject.name.Equals("tierRaiseButton"))
            {
                tierRaiseButton = button;
                tierRaiseButton.onClick.AddListener(TierRaiseCallback);
            }
            else if (button.gameObject.name.Equals("tierLowerButton"))
            {
                tierLowerButton = button;
                tierLowerButton.onClick.AddListener(TierLowerCallback);
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (gameController.dGame.currentCity != null)
        {
            text.text = "Shelter Tier: " + gameController.dGame.currentCity.ShelterTier + " (+" + gameController.dGame.currentCity.FuelToShelterConversion + ")";
        }
    }

    public void TierRaiseCallback()
    {
        if (gameController.dGame.currentCity != null)
        {
            gameController.dGame.currentCity.RaiseShelterTier();
			GameObject.Find ("SfxLibrary").GetComponents<AudioSource>() [5].Play ();
        }
    }

    public void TierLowerCallback()
    {
        if (gameController.dGame.currentCity != null)
        {
            gameController.dGame.currentCity.LowerShelterTier();
			GameObject.Find ("SfxLibrary").GetComponents<AudioSource>() [4].Play ();
        }
    }
}
