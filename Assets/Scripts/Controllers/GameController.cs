using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public DGame dGame = new DGame();


    // Initialization
    void Start()
    {
        CreateCity("Ottawa");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dGame.EndTurnUpdate();
        }
    }

    public void CreateCity(string cityName)
    {        
        CityController cityController = InstantiatePrefab<CityController>(Constants.OTTAWA_PREFAB_PATH);
        cityController.ConnectToDataEngine(dGame, cityName);        

        // This will be refactored into some sort of text file (csv, json, etc)
        CreateBuilding("Ottawa", "Town Hall", new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 1));
        CreateBuilding("Ottawa", "Apartment Building", new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 1));
        CreateBuilding("Ottawa", "Derelict Building", new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 1));
        CreateBuilding("Ottawa", "Library", new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 1));
        CreateBuilding("Ottawa", "Grocery Store", new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 1));
                
    }

    public void CreateBuilding(string cityName, string buildingName, Vector3 position)
    {
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH);
        buildingController.ConnectToDataEngine(dGame, cityName, buildingName);
        
        buildingController.transform.position = position;
    }

    private T InstantiatePrefab<T>(string prefabPath)
    {
        return (Instantiate(Resources.Load(prefabPath)) as GameObject).GetComponent<T>();
    }
}
