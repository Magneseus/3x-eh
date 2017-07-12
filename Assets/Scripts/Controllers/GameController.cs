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

    }

    public void EndTurnButtonCallback()
    {
        dGame.EndTurnUpdate();
    }

    public CityController CreateCity(string prefabPath, string jsonPath)
    {
        var cityJson = JSON.Parse(Resources.Load<TextAsset>(jsonPath).text);

        CityController cityController = InstantiatePrefab<CityController>(Constants.OTTAWA_PREFAB_PATH);
        cityController.ConnectToDataEngine(dGame, cityJson["name"]);

        // Load in all buildings for the city
        foreach(JSONNode building in cityJson["buildings"].AsArray)
        {
            // Get the random positions available
            JSONNode xPos = building["position"]["x"].AsArray;
            JSONNode yPos = building["position"]["y"].AsArray;

            BuildingController bControl = CreateBuilding(cityJson["name"], building["name"], new Vector3(Random.Range(xPos[0], xPos[1]), Random.Range(yPos[0], yPos[1]), 1));

            // Load in all the tasks for this building
            foreach (JSONNode task in building["tasks"].AsArray)
            {
                // TODO: Check for "special" tasks, like assess/explore/etc.
                string taskName = task["name"];
                int maxPeople = task["maxPeople"];

                // Load in the resource output for this task
                DResource taskResource = DResource.Create(
                    task["resource"]["name"],
                    task["resource"]["amount"]);

                DTask newTask = new DTask(
                    bControl.dBuilding,
                    taskResource,
                    maxPeople,
                    taskName);

                // Generate the task controller and attach it
                TaskController newTaskController = AttachTaskController(newTask, bControl);
            }
        }
        foreach(JSONNode resource in cityJson["resources"].AsArray)
        {
            DResource r = DResource.Create(resource["name"], resource["amount"]);
            cityController.dCity.AddResource(r);
        }
        // MAP OF CANADA STUFF
          List<string> edges = new List<string>();
          int i = 0;
          foreach(JSONNode edge in cityJson["edges"].AsArray) edges.Add(edge);
          cityController.dCity.setEdges(edges);
        //TODO: Remove this
        CreateMeeple(cityJson["name"]);

        return cityController;
    }

    public BuildingController CreateBuilding(string cityName, string buildingName, Vector3 position)
    {
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH);
        buildingController.ConnectToDataEngine(dGame, cityName, buildingName);

        buildingController.transform.position = position;

        return buildingController;
    }

    // TODO: Spawn in "empty building"
    public MeepleController CreateMeeple(string cityName)
    {
        MeepleController meepleController = InstantiatePrefab<MeepleController>(Constants.MEEPLE_PREFAB_PATH);
        meepleController.ConnectToDataEngine(dGame, cityName);

        return meepleController;
    }

    public TaskController AttachTaskController(DTask dTask, BuildingController buildingController)
    {
        TaskController taskController = InstantiatePrefab<TaskController>(Constants.TASK_TRAY_PREFAB_PATH, buildingController.transform);
        taskController.ConnectToDataEngine(dTask);

        // Give BuildingController a reference to it and vice versa
        buildingController.AddTaskController(taskController);
        taskController.buildingController = buildingController;

        return taskController;
    }

    private T InstantiatePrefab<T>(string prefabPath)
    {
        return (Instantiate(Resources.Load(prefabPath)) as GameObject).GetComponent<T>();
    }

    private T InstantiatePrefab<T>(string prefabPath, Transform parent)
    {
        return (Instantiate(Resources.Load(prefabPath), parent) as GameObject).GetComponent<T>();
    }
}
