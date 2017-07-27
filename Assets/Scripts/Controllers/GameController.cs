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
    private CountryMap countryMap;
    private GameObject cityView;

    // Initialization
    void Start()
    {
        dGame = new DGame(this);

        // Start off at the country view
        countryView = Instantiate(CountryViewUIPrefab, UICanvas.transform);
        countryMap = countryView.GetComponent<CountryMap>();

        // Get all cities
        foreach (string file in System.IO.Directory.GetFiles(Constants.CITY_JSON_PATH))
        {
            if (Path.GetExtension(file) == ".json")
            {
                var cityJSON = JSON.Parse(File.ReadAllText(file));

                List<string> edges = new List<string>();
                for(int i=0; i< cityJSON["edges"].AsArray.Count; i++)
                {
                   edges.Add((cityJSON["edges"].AsArray[i]));
                }
                countryMap.SpawnCityNode(
                    cityJSON["name"],
                    new Vector3(cityJSON["position"]["x"],cityJSON["position"]["y"], -1),
                    edges);

            }
        }
        countryMap.SpawnEdges();
    }

    void Update()
    {
        if (Input.GetKeyUp("s"))
        {
            File.WriteAllText(@"Assets/Resources/testLoad.json", dGame.SaveToJSON().ToString());
        }
        else if (Input.GetKeyUp("l"))
        {
            LoadGame();
        }
    }

    public void LoadGame()
    {
        var json = File.ReadAllText(@"Assets/Resources/testLoad.json");
        dGame = DGame.LoadFromJSON(JSON.Parse(json), this);

        if (dGame.currentCity != null)
        {
            // Spawn City UI
            cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);

            // Disable Country View
            countryView.SetActive(false);
        }
    }

    public void SelectCity(string cityName)
    {
        CreateCity(Constants.CITY_JSON_PATH, File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json"));

        dGame.SelectCity(cityName);
        // dGame.currentCity.CityController.assignGameController(this);

        // Spawn City UI
        cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);

        // Disable Country View
        countryView.SetActive(false);
    }

    public void ReturnToMap(bool destroyCityView=false)
    {
        if (destroyCityView)
        {
            Destroy(cityView);

            // Destroy game objects
            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));

            // Disable all cities
            countryMap.DisableAllNodes();

            // Enable cities connected to completed city
            List<string> linkedCities = new List<string>();
            foreach (var cityName in dGame.currentCity.LinkedCityKeys)
            {
                // If we haven't previously completed this city
                if (!dGame.Cities.ContainsKey(cityName))
                    linkedCities.Add(cityName);
            }

            countryMap.SetCitiesEnabled(linkedCities, true);

            // Reset the turn counter
            dGame.TurnNumber = 0;
        }

        countryView.SetActive(true);
    }

    public void EndTurnButtonCallback()
    {
        dGame.EndTurnUpdate();
    }

    #region Controller Spawners

    public CityController CreateCityController(DCity city)
    {
        CityController cityController = InstantiatePrefab<CityController>(Constants.CITY_PREFAB_PATH, this.transform);
        cityController.ConnectToDataEngine(dGame, city);

        return cityController;
    }

    public BuildingController CreateBuildingController(DBuilding building, Vector3 position)
    {
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH, this.transform);
        buildingController.ConnectToDataEngine(building);

        // Set position
        buildingController.transform.position = position;

        // Generate all predefined tasks
        foreach (var kvp in buildingController.dBuilding.Tasks)
        {
            TaskController newTaskController = AttachTaskController(kvp.Value, buildingController);
        }

        return buildingController;
    }

    public MeepleController CreateMeepleController(TaskTraySingle taskTray, DPerson person)
    {
        MeepleController meepleController = InstantiatePrefab<MeepleController>(Constants.MEEPLE_PREFAB_PATH, taskTray.transform);
        meepleController.ConnectToDataEngine(taskTray, person);

        return meepleController;
    }

    #endregion

    public CityController CreateCity(string prefabPath, string json)
    {
        var cityJson = JSON.Parse(json);
        CityController cityController = InstantiatePrefab<CityController>(Constants.CITY_PREFAB_PATH, this.transform);
        cityController.ConnectToDataEngine(dGame, cityJson["name"]);

        // Load in all the possible locations for buildings
        List<Vector2> possibleBuildingLocations = new List<Vector2>();
        foreach (JSONNode buildingLoc in cityJson["building_locations"].AsArray)
        {
            // Get the random positions available
            float xPos = buildingLoc["x"].AsFloat;
            float yPos = buildingLoc["y"].AsFloat;

            possibleBuildingLocations.Add(new Vector2(xPos, yPos));
        }

        // Load in all buildings for the city
        foreach(JSONNode building in cityJson["buildings"].AsArray)
        {
            //TODO: Check if building has a set position?

            Vector2 location = new Vector2(0, 0);

            // Temporary fix for townhall placement
            if (!building["name"].Equals("Town Hall"))
            {
                // Pull a random building location from the list
                int randIndex = Mathf.RoundToInt(Random.Range(0, possibleBuildingLocations.Count - 1));
                location = possibleBuildingLocations[randIndex];
                possibleBuildingLocations.RemoveAt(randIndex);
            }

            BuildingController bControl = CreateBuilding(cityJson["name"], building["name"], new Vector3(location.x, location.y, 1));
			if(building["name"].Equals("Town Hall"))
				bControl.dBuilding.Assess(1.0f);
            // Load in all the tasks for this building
            foreach (JSONNode task in building["tasks"].AsArray)
            {

                // TODO: Check for "special" tasks, like assess/explore/etc.
                string taskName = task["name"];
                int maxPeople = task["maxPeople"];
                float fullAssessmentRequirement = task["fullAssess"];

                // Load in the resource output for this task
                DResource taskResource = DResource.Create(
                    task["resource"]["name"],
                    task["resource"]["amount"]);

                DTask newTask = new DTask(
                    bControl.dBuilding,
                    taskResource,
                    maxPeople,
                    taskName,
                    fullAssessmentRequirement);
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
        BuildingController buildingController = InstantiatePrefab<BuildingController>(Constants.BUILDING_PREFAB_PATH, this.transform);
        buildingController.ConnectToDataEngine(dGame, cityName, buildingName);

        buildingController.transform.position = position;

        // Generate all predefined tasks
        foreach (var kvp in buildingController.dBuilding.Tasks)
		{
            TaskController newTaskController = AttachTaskController(kvp.Value, buildingController);
        }

        return buildingController;
    }

    // TODO: Spawn in "empty building"
    public MeepleController CreateMeeple(string cityName)
    {
        MeepleController meepleController = InstantiatePrefab<MeepleController>(Constants.MEEPLE_PREFAB_PATH, this.transform);
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
