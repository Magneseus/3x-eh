using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{

    public GameManager gameManager = new GameManager();

    // Initialization
    void Start()
    {
       
    }


    //Updated per frame, use for UI.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            gameManager.EndTurnUpdate();
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
