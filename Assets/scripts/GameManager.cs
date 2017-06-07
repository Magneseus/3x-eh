
 using System.Collections;

 using System.Collections.Generic;

 using UnityEngine;



 

 


public class GameManager : MonoBehaviour {



    // Maybe we want this to be a reference to a Map?

    public List<City> ListOfCities;

    public int DurationOfTurn = 7;

    private int CurrentTurnNumber;

    private int DaysTranspired;

 

 

    // Initialization

	// Use this for initialization


	void Start () {

    

		

        CurrentTurnNumber = 0;

	
        DaysTranspired = 0;

	

     	}

		

    void EndTurnUpdate()
    {

        // Here we will update everything (basically just updating the cities)

        foreach (City c in ListOfCities)

        {

            c.TurnUpdate(DurationOfTurn);        

        }

        CurrentTurnNumber += 1;

        DaysTranspired += DurationOfTurn;

        Debug.Log("Turn ended, " + DurationOfTurn + " days passed.");

    }



    //Updated per frame, use for UI.

    void Update()

    {

        if (Input.GetKeyDown(KeyCode.E))

        {

            EndTurnUpdate();

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
