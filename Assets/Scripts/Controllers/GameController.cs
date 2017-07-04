using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameController : MonoBehaviour
{

    public DGame dGame = new DGame();


    // Initialization
    void Start()
    {        
        CreateCity(Constants.OTTAWA_PREFAB_PATH, Constants.OTTAWA_JSON_PATH);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dGame.EndTurnUpdate();
        }
    }

    public CityController CreateCity(string prefabPath, string jsonPath)
    {
        var cityJson = JSON.Parse(Resources.Load<TextAsset>(jsonPath).text);

        CityController cityController = InstantiatePrefab<CityController>(Constants.OTTAWA_PREFAB_PATH);        
        cityController.ConnectToDataEngine(dGame, cityJson["name"]);     
        
        foreach(JSONNode building in cityJson["buildings"].AsArray)
        {
            JSONNode xPos = building["position"]["x"].AsArray;
            JSONNode yPos = building["position"]["y"].AsArray;

            BuildingController bControl = CreateBuilding(cityJson["name"], building["name"], new Vector3(Random.Range(xPos[0], xPos[1]), Random.Range(yPos[0], yPos[1]), 1));

            foreach (JSONNode task in building["tasks"].AsArray)
            {
                string taskName = task["name"];
                int maxPeople = task["maxPeople"];

                DResource taskResource = DResource.Create(
                    task["resource"]["name"],
                    task["resource"]["amount"]);

                DTask newTask = new DTask(
                    bControl.dBuilding, 
                    taskResource, 
                    maxPeople, 
                    taskName);
            }
        }

        return cityController;
    }

    public BuildingController CreateBuilding(string cityName, string buildingName, Vector3 position)
    {
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH);
        buildingController.ConnectToDataEngine(dGame, cityName, buildingName);
        
        buildingController.transform.position = position;

        return buildingController;
    }

    private T InstantiatePrefab<T>(string prefabPath)
    {
        return (Instantiate(Resources.Load(prefabPath)) as GameObject).GetComponent<T>();
    }
}
