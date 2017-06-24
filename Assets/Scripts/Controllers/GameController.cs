using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public DGame dGame = new DGame();

    

    // Initialization
    void Start()
    {
        InitializeCity();
    }

    private void InitializeCity()
    {
        DCity dCity = new DCity();
        dCity.Name = "Ottawa Sprint 1";

        CityController cityController = (Instantiate(Resources.Load("Prefabs/Cities/Ottawa")) as GameObject).GetComponent<CityController>();
        cityController.dCity = dCity;
        dCity.CityController = cityController;
    }

    //Updated per frame, use for UI.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dGame.EndTurnUpdate();
        }
    }

    //////////// Single Instance Assertion Stuff
    private static bool _isInstantiated = false;

    private void Awake()
    {
        // IF THIS FAILS, we've instantiated another GameManager somewhere!!
        Debug.Assert(!_isInstantiated);
        _isInstantiated = true;
    }

    private void OnDestroy()
    {
        _isInstantiated = false;
    }
    ///// End of Single Instance Assertion Stuff
}
