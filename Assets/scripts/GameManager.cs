using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Maybe we want this to be a reference to a Map?
    public List<City> ListOfCities;

    // Initialization
    void Start()
    {
        
    }

    // Turn Update
    void EndTurnUpdate()
    {
        // Here we will update everything (basically just updating the cities)
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
