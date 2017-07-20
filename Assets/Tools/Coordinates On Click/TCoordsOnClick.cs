using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCoordsOnClick : MonoBehaviour
{
    public GameObject markerToSpawn;
    public string inputToPress;

    private List<GameObject> spawnedMarkers;
    private GameObject currentMarker;

	// Use this for initialization
	void Start ()
    {
        spawnedMarkers = new List<GameObject>();
        currentMarker = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
        int attemptParse = -1;
        int.TryParse(inputToPress, out attemptParse);
        bool mousePress = attemptParse != -1 ? Input.GetMouseButton(attemptParse) : false;

        bool keyPress = false;
        try
        {
            keyPress = Input.GetKey(inputToPress);
        }
        catch (System.ArgumentException ae) { }

        bool inputPressed = keyPress;


        if (inputPressed && currentMarker == null)
        {
            currentMarker = Instantiate(markerToSpawn, this.transform);
        }




        if (currentMarker != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;
            currentMarker.transform.position = mousePos;
        }




        mousePress = attemptParse != -1 ? Input.GetMouseButtonUp(attemptParse) : false;

        keyPress = false;
        try
        {
            keyPress = Input.GetKeyUp(inputToPress);
        }
        catch (System.ArgumentException ae) { }

        inputPressed = keyPress;

        if (inputPressed)
        {
            spawnedMarkers.Add(currentMarker);
            currentMarker = null;

            PrintMarkerPositions();
        }
    }

    public string PrintMarkerPositions()
    {
        string printout = "[\n";

        foreach (var marker in spawnedMarkers)
        {
            printout += "    ";
            printout += "{ \"x\": " + marker.transform.position.x.ToString("0.0");
            printout += ", \"y\": " + marker.transform.position.y.ToString("0.0");
            printout += "},\n";
        }

        printout = printout.Remove(printout.Length - 2);
        printout += "\n]";

        Debug.Log(printout);

        return printout;
    }
}
