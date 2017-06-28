using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public DGame dGame = new DGame();


    // Initialization
    void Start()
    {
        InitializeCities();

    }

    private void InitializeCities()
    {
        CreateCity("Ottawa");        
        
    }

    public void CreateCity(string cityName)
    {        
        CityController cityController = InstantiatePrefab<CityController>(Constants.OTTAWA_PREFAB_PATH);
        DCity dCity = new DCity(cityName, cityController);
        cityController.dCity = dCity;

        dGame.Cities.Add(cityName, dCity);

        // Create the town hall
        CreateBuilding("Ottawa", "Town Hall");
    }

    public void CreateBuilding(string cityName, string buildingName)
    {
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH);
        DBuilding dBuilding = new DBuilding(dGame.Cities[cityName], buildingName, buildingController);
        buildingController.dBuilding = dBuilding;   
    }

    //Updated per frame, use for UI.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dGame.EndTurnUpdate();
        }
    }

    private T InstantiatePrefab<T>(string prefabPath)
    {
        return (Instantiate(Resources.Load(prefabPath)) as GameObject).GetComponent<T>();
    }
}
