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

        // Re-enable the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

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
                for(int i=0; i< cityJSON["linked_cities"].AsArray.Count; i++)
                {
                   edges.Add((cityJSON["linked_cities"].AsArray[i]));
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
            //File.WriteAllText(@"Assets/Resources/testLoad.json", dGame.SaveToJSON().ToString());
            SaveGame("test.json");
        }
        else if (Input.GetKeyUp("l"))
        {
            //LoadGame();
            LoadGame("test.json");
        }
    }

    public void LoadGame(string savedGameFile, string pathToSavedGames=Constants.SAVE_JSON_PATH)
    {
        var json = File.ReadAllText(pathToSavedGames + @"/" + savedGameFile);
        dGame = DGame.LoadFromJSON(JSON.Parse(json), this);

        if (dGame.currentCity != null)
        {
            // Spawn City UI
            cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);

            // Disable Country View
            countryView.SetActive(false);
        }
    }

    public void SaveGame(string savedGameFile, string pathToSavedGames = Constants.SAVE_JSON_PATH)
    {
        var json = dGame.SaveToJSON();
        File.WriteAllText(pathToSavedGames + @"/" + savedGameFile, json.ToString());
    }

    public void SelectCity(string cityName)
    {
        //CreateCity(Constants.CITY_JSON_PATH, File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json"));

        var json = File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json");
        cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);
        DCity newCity = DCity.LoadFromJSON(JSON.Parse(json), dGame, true);
        
        dGame.AddCity(newCity);
        dGame.SelectCity(cityName);
        

        // Spawn City UI
        

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
		GameObject.Find ("SfxLibrary").GetComponents<AudioSource>()[2].Play();
    }

    #region Controller Spawners

    public CityController CreateCityController(DCity city)
    {
        CityController cityController = InstantiatePrefab<CityController>(Constants.CITY_PREFAB_PATH, this.transform);
        cityController.ConnectToDataEngine(dGame, city);

        return cityController;
    }

    public BuildingController CreateBuildingController(DBuilding building, Vector3 position, Constants.BUILDING_TYPE buildType = Constants.BUILDING_TYPE.MEDIUM)
    {
        string prefab_path = RandomBuildingPrefabPath(buildType);
        BuildingController buildingController = InstantiatePrefab<BuildingController>(prefab_path, this.transform);
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

    public string RandomBuildingPrefabPath(Constants.BUILDING_TYPE buildType)
    {
        float roll = Random.value;
        string result;
        switch (buildType)
        {
            case Constants.BUILDING_TYPE.MEDIUM:
                float interval = 1f / Constants.BUILDING_PREFAB_PATHS_MED.Length;
                int index = (int)Mathf.Floor(roll / interval);
                result = Constants.BUILDING_PREFAB_PATHS_MED[index];
                break;
            case Constants.BUILDING_TYPE.IQALUIT_HALL:
                result = Constants.BUILDING_PREFAB_PATH_IQALUIT_HALL;
                break;
            case Constants.BUILDING_TYPE.IQALUIT_AIRPORT:
                result = Constants.BUILDING_PREFAB_PATH_IQALUIT_AIRPORT;
                break;
            case Constants.BUILDING_TYPE.IQALUIT_NAKASUK:
                result = Constants.BUILDING_PREFAB_PATH_IQALUIT_NAKASUK;
                break;
            default:
                result = Constants.BUILDING_PREFAB_PATH;
                break;
        }
        
        return result;
    }

    public MeepleController CreateMeepleController(TaskTraySingle taskTray, DPerson person)
    {
        MeepleController meepleController = InstantiatePrefab<MeepleController>(Constants.MEEPLE_PREFAB_PATH, taskTray.transform);
        meepleController.ConnectToDataEngine(taskTray, person);

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

    #endregion

    private T InstantiatePrefab<T>(string prefabPath)
    {
        return (Instantiate(Resources.Load(prefabPath)) as GameObject).GetComponent<T>();
    }

    private T InstantiatePrefab<T>(string prefabPath, Transform parent)
    {
        return (Instantiate(Resources.Load(prefabPath), parent) as GameObject).GetComponent<T>();
    }
}
