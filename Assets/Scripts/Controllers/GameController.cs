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

    private bool buildingToggle = false;
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

                // Add city to list of available cities for the game
                dGame.availableCities.Add(cityJSON["name"]);

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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ToggleBuildingModals(!buildingToggle);
            buildingToggle = !buildingToggle;
        }
    }

    public void LoadGame(string savedGameFile, string pathToSavedGames=Constants.SAVE_JSON_PATH)
    {
        pathToSavedGames = Application.dataPath + pathToSavedGames;

        destroyCityAndBuildings();
        Destroy(cityView);
        var json = File.ReadAllText(pathToSavedGames + @"/" + savedGameFile);
        dGame = DGame.LoadFromJSON(JSON.Parse(json), this);

        if (dGame.currentCity != null)
        {

            // Spawn City UI
            cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);

            // Disable Country View
            countryView.SetActive(false);
        }
        else
        {
            ReturnToMap(true);
        }
    }

    public void SaveGame(string savedGameFile, string pathToSavedGames = Constants.SAVE_JSON_PATH)
    {
        pathToSavedGames = Application.dataPath + pathToSavedGames;

        // Add the .json extension if not present
        if (!savedGameFile.EndsWith(".json"))
        {
            savedGameFile += ".json";
        }

        var json = dGame.SaveToJSON();
        File.WriteAllText(pathToSavedGames + @"/" + savedGameFile, json.ToString());
    }

    public void DeleteGame(string deleteGameFile, string pathToSavedGames = Constants.SAVE_JSON_PATH)
    {
        pathToSavedGames = Application.dataPath + pathToSavedGames;

        // Add the .json extension if not present
        if (!deleteGameFile.EndsWith(".json"))
        {
            deleteGameFile += ".json";
        }
        File.Delete(pathToSavedGames + Path.DirectorySeparatorChar + deleteGameFile);
        File.Delete(pathToSavedGames + Path.DirectorySeparatorChar + deleteGameFile.Replace(".json",".meta"));
    }

    public void SelectCity(string cityName)
    {
        //CreateCity(Constants.CITY_JSON_PATH, File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json"));

        var json = File.ReadAllText(Constants.CITY_JSON_PATH + @"/" + cityName.ToLower() + ".json");
        cityView = Instantiate(CityViewUIPrefab, UICanvas.transform);
        DCity newCity = DCity.LoadFromJSON(JSON.Parse(json), dGame, false);

        dGame.AddCity(newCity);
        dGame.SelectCity(cityName);


        // Spawn City UI


        // Disable Country View
        countryView.SetActive(false);
    }

    public List<string> listSavedGames(string pathToSavedGames = Constants.SAVE_JSON_PATH)
    {
        pathToSavedGames = Application.dataPath + pathToSavedGames;

        List<string> listSavedGames = new List<string>();
        // Debug.Log();

        foreach(string s in Directory.GetFiles(pathToSavedGames, "*.json"))
        {
            string[] newS = s.Split(Path.DirectorySeparatorChar);
            string name = newS[newS.Length-1];
            listSavedGames.Add(name.Split('.')[0]);
        }

        return listSavedGames;
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

            if (dGame.currentCity != null)
            {
                // Enable cities connected to completed city
                List<string> linkedCities = new List<string>();
                foreach (var cityName in dGame.currentCity.LinkedCityKeys)
                {
                    // If we haven't previously completed this city
                    if (!dGame.Cities.ContainsKey(cityName))
                        linkedCities.Add(cityName);
                }

                countryMap.SetCitiesEnabled(linkedCities, true);
            }
            else
            {
                // Only show available cities
                countryMap.SetCitiesEnabled(dGame.availableCities, true);
            }

            // Reset the turn counter
            dGame.TurnNumber = 0;
        }

        countryView.SetActive(true);
    }

    public void EndTurnButtonCallback()
    {
        if (dGame.GameState == DGame._gameState.PLAY)
        {
            dGame.EndTurnUpdate();
            GameObject.Find("SfxLibrary").GetComponents<AudioSource>()[2].Play();
        }
    }
    public void NewGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cityView != null)
        {
            destroyCityAndBuildings();
            Destroy(cityView);
            dGame.Reset();
        }
        countryView.SetActive(true);



        foreach (string file in System.IO.Directory.GetFiles(Constants.CITY_JSON_PATH))
        {
            if (Path.GetExtension(file) == ".json")
            {
                var cityJSON = JSON.Parse(File.ReadAllText(file));

                List<string> edges = new List<string>();
                for (int i = 0; i < cityJSON["linked_cities"].AsArray.Count; i++)
                {
                    edges.Add((cityJSON["linked_cities"].AsArray[i]));
                }
                countryMap.SpawnCityNode(
                    cityJSON["name"],
                    new Vector3(cityJSON["position"]["x"], cityJSON["position"]["y"], -1),
                    edges);

            }
        }
        countryMap.SpawnEdges();
    }


    public void destroyCityAndBuildings()
    {
        foreach (Transform t in this.transform)
        {
            Destroy(t.gameObject);
        }
        Destroy(cityView);
    }

    public void ToggleBuildingModals(bool toggle)
    {
        // Toggle all building controllers
        var children = new List<BuildingController>();
        foreach (Transform child in transform)
        {
            BuildingController bc = child.gameObject.GetComponent<BuildingController>();
            if (bc != null)
                children.Add(bc);
        }
        children.ForEach(child => child.ToggleBuildingModal(toggle));
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
        string prefab_path = DBuilding.RandomBuildingPrefabPath(building.BuildType);
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
