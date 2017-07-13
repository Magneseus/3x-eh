using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class GameController : MonoBehaviour
{
    public DGame dGame;

    public GameObject UICanvas;
    public GameObject CountryViewUIPrefab;
    public GameObject CityViewUIPrefab;

    private GameObject countryView;
    private GameObject cityView;

    // Initialization
    void Start()
    {
        dGame = new DGame();

        // Start off at the country view
        countryView = Instantiate(CountryViewUIPrefab, UICanvas.transform);
        CountryMap countryMap = countryView.GetComponent<CountryMap>();

        // Get all cities
        foreach (string file in System.IO.Directory.GetFiles(Constants.CITY_JSON_PATH))
        {
            if (Path.GetExtension(file) == ".json")
            {
                var cityJSON = JSON.Parse(File.ReadAllText(file));

                countryMap.SpawnCityNode(
                    cityJSON["name"],
                    new Vector3(cityJSON["position"]["x"],cityJSON["position"]["y"], -1),
                    new List<string>(cityJSON["edges"].AsArray));
            }
        }
    }

    void Update()
    {

    }

    public void SelectCity(string cityName)
    {
      // Debug.Log(Path.DirectorySeparatorChar);
        CreateCity(Constants.CITY_JSON_PATH, File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json"));
        dGame.SelectCity(cityName);

        // Spawn City UI
        cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);

        // Disable Country View
        countryView.SetActive(false);
    }

    public void EndTurnButtonCallback()
    {
        dGame.EndTurnUpdate();
    }

    public CityController CreateCity(string prefabPath, string json)
    {
        var cityJson = JSON.Parse(json);
        CityController cityController = InstantiatePrefab<CityController>(Constants.CITY_PREFAB_PATH);
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
        foreach (JSONNode edge in cityJson["edges"].AsArray) edges.Add(edge);
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
