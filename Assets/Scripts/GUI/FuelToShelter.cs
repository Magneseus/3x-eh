using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelToShelter : MonoBehaviour
{
    private Text text;
    private Button fuelRaiseButton;
    private Button fuelLowerButton;
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        text = GetComponent<Text>();
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            if (button.gameObject.name.Equals("fuelRaiseButton"))
            {
                fuelRaiseButton = button;
                fuelRaiseButton.onClick.AddListener(FuelRaiseCallback);
            }
            else if (button.gameObject.name.Equals("fuelLowerButton"))
            {
                fuelLowerButton = button;
                fuelLowerButton.onClick.AddListener(FuelLowerCallback);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.dGame.currentCity != null)
        {
            text.text = "Fuel Use: " + gameController.dGame.currentCity.FuelToShelterConversion;
        }
    }

    public void FuelRaiseCallback()
    {
        if (gameController.dGame.currentCity != null)
        {
            gameController.dGame.currentCity.RaiseFuelConversion();
			GameObject.Find ("SoundLibrary").GetComponents<AudioSource>() [5].Play ();
        }
    }

    public void FuelLowerCallback()
    {
        if (gameController.dGame.currentCity != null)
        {
            gameController.dGame.currentCity.LowerFuelConversion();
			GameObject.Find ("SoundLibrary").GetComponents<AudioSource>() [4].Play ();
        }
    }
}
