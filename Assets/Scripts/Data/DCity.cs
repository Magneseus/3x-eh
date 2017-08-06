using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class DCity : ITurnUpdatable
{
    [NonSerialized]
    private DGame dGame;

    private CityController cityController;
    private Dictionary<int, DBuilding> buildings = new Dictionary<int, DBuilding>();
    private Dictionary<int, DResource> resources = new Dictionary<int, DResource>();
    private Dictionary<int, DResource> prevResources = new Dictionary<int, DResource>();

    private Dictionary<int, DResource> resourceRates = new Dictionary<int, DResource>();

    private Dictionary<int, DPerson> people = new Dictionary<int, DPerson>();
    private List<string> linkedCityKeys = new List<string>();

    private int age;
    private string name;
    private int shelterTier;
    private int fuelToShelterConversion;
    private DResource shelterResource;
    private DResource fuelResource;

    private float explorationLevel;
    public DBuilding townHall;

    private DSeasons._season season;
    private DateTime[] seasonStartDates = new DateTime[4];
    private DateTime[] deadOfWinterStartEnd = new DateTime[2];
    private bool isDeadOfWinter = false;

    public static float health = 0.5f;
    public static int foodConsumption = 1;
    public static float notEnoughFoodHealthDecay = 0.8f;

    #region Constructors and Init
    public DCity(string cityName, CityController cityController)
    {
        Init(cityName, cityController, DSeasons.DefaultStartDates(), new DateTime(2017,1,1), DSeasons.DefaultDeadOfWinterDates(), null);
    }

    public DCity(string cityName, CityController cityController, DateTime currentDate)
    {
        Init(cityName, cityController, DSeasons.DefaultStartDates(), currentDate, DSeasons.DefaultDeadOfWinterDates(), null);
    }

    public DCity(string cityName, CityController cityController, DateTime[] seasonDates, DateTime currentDate, List<string> linkedCityKeys = null)
    {
        Init(cityName, cityController, seasonDates, currentDate, DSeasons.DefaultDeadOfWinterDates(), linkedCityKeys);
    }

    private void Init(string cityName, CityController cityController, DateTime[] seasonDates, DateTime currentDate,
         DateTime[] deadWinterDates, List<string> linkedCityKeys)
    {
        name = cityName;
        this.cityController = cityController;
        age = 0;
        townHall = null;
        explorationLevel = 0.0f;
        shelterTier = 1;
        fuelToShelterConversion = 0;

        InitialLinkedCities(linkedCityKeys);
        deadOfWinterStartEnd = deadWinterDates;
        seasonStartDates = DSeasons.InitialSeasonSetup(seasonDates, currentDate, ref season, ref deadOfWinterStartEnd);
    }

    private void InitialLinkedCities(List<string> linkedCityKeys)
    {
        this.linkedCityKeys = new List<string>();
        if (linkedCityKeys != null)
            foreach (string cityKey in linkedCityKeys)
                this.linkedCityKeys.Add(cityKey);
    }

    private void InitialDeadOfWinter(DateTime currentDate, DateTime[] deadWinterDates)
    {
        deadOfWinterStartEnd = deadWinterDates;
        if (currentDate < deadOfWinterStartEnd[1] && currentDate >= deadOfWinterStartEnd[0])
            isDeadOfWinter = true;
        else
            isDeadOfWinter = false;
    }
    #endregion

    #region Update Calls
    // TurnUpdate is called once per Turn
    public void TurnUpdate(int numDaysPassed)
    {
        // Set shelter resource to zero, cannot accumulate shelter
        prevResources.Clear();
        foreach(var entry in resources)
        {
          prevResources.Add(entry.Key,  DResource.Create(entry.Value));
          prevResources[entry.Key].Amount = entry.Value.Amount;
        }
        if (shelterResource != null)
            shelterResource.Amount = 0;

        UpdateBuildings(numDaysPassed);
        UpdateResources(numDaysPassed);
        UpdatePeople(numDaysPassed);
        UpdateCity();

        if (shelterResource != null)
        {
            // Shelter calculations
            int shelterResourceAmt = shelterResource.Amount;
            shelterResourceAmt = shelterResourceAmt - ShelterConsumedPerTurn();
            // TODO: Deal with negative shelter amounts
        }

        if (fuelResource != null)
        {
            // Check if our fuel consumption is exceeding our current fuel stores
            fuelToShelterConversion = fuelToShelterConversion < fuelResource.Amount ? fuelToShelterConversion : fuelResource.Amount;
        }

        age += numDaysPassed;
    }

    // TODO: Account for infection in people
    public int ShelterConsumedPerTurn()
    {
        int amountShelterPerPerson = Mathf.RoundToInt(Mathf.Pow(2.0f, shelterTier - 1));

        return amountShelterPerPerson * people.Count;
    }

    public int ShelterNetTier()
    {
        return Mathf.Clamp(shelterTier + fuelToShelterConversion, 1, 5);
    }

    public void UpdateSeason(DateTime currentDate)
    {
        seasonStartDates = DSeasons.UpdateSeasonStatus(seasonStartDates, currentDate, ref season);
        UpdateDeadOfWinter(currentDate);
    }

    public void UpdateDeadOfWinter(DateTime currentDate)
    {
        if (!isDeadOfWinter)
            isDeadOfWinter = DSeasons.StartDeadOfWinter(ref deadOfWinterStartEnd, currentDate);
        else
            isDeadOfWinter = !DSeasons.EndDeadOfWinter(ref deadOfWinterStartEnd, currentDate);
    }

    #region Update Other Elements
    private void UpdateBuildings(int numDaysPassed)
    {
        foreach (var entry in buildings)
        {
            entry.Value.TurnUpdate(numDaysPassed);
            BuildingSeasonalEffects(entry.Value, numDaysPassed);
        }
    }

    private void BuildingSeasonalEffects(DBuilding building, int numDaysPassed)
    {
        switch (season)
        {
            case DSeasons._season.SPRING:
                building.SpringEffects();
                break;
            case DSeasons._season.SUMMER:
                building.SummerEffects();
                break;
            case DSeasons._season.FALL:
                building.FallEffects();
                break;
            case DSeasons._season.WINTER:
                building.WinterEffects();
                break;
        }
    }

    private void UpdateResources(int numDaysPassed)
    {
        foreach (var entry in resources)
        {
            entry.Value.TurnUpdate(numDaysPassed);

            // Remove fuel due to shelter conversion
            if (entry.Value.Name.Equals("Fuel"))
            {
                entry.Value.Amount -= fuelToShelterConversion;
            }
        }
        CalculateResourceRates();

    }

    private void UpdatePeople(int numDaysPassed)
    {
        int exploringInWinter = 0;

        List<DPerson> listOfDeadPeople = new List<DPerson>();
        foreach (var entry in people.Keys)
        {
            people[entry].TurnUpdate(numDaysPassed);
            UpdatePeopleWinter(people[entry], ref exploringInWinter);

            if (people[entry].IsDead)
                listOfDeadPeople.Add(people[entry]);
        }

        // Remove dead people
        foreach (var person in listOfDeadPeople)
        {
            people.Remove(person.ID);
        }

        if (exploringInWinter > 1)
            health = Mathf.Clamp(health - DSeasons.reduceHealthExploringWinter, 0f, 1f);
    }

    private void UpdatePeopleWinter(DPerson person, ref int exploringInWinter)
    {
        if (person.Task != null && person.Task.GetType() == typeof(DTask_Explore) && season == DSeasons._season.WINTER)
        {
            exploringInWinter++;
            DeadOfWinterCulling(person);
        }
    }

    public void DeadOfWinterCulling(DPerson person)
    {
        if (isDeadOfWinter)
        {
            person.Dies();
        }
    }
    #endregion

    public void UpdateCity()
    {
        UpdateCityFood();
        UpdateCityHealth();
    }

    public void UpdateCityFood()
    {
        DResource resource = GetResource(Constants.FOOD_RESOURCE_NAME);
        int consumeAmount = (int)(foodConsumption * DSeasons.modFoodConsumption[(int)season]);
        if (resource.Amount >= foodConsumption)
            ConsumeResource(resource, foodConsumption);
        else
        {
            ConsumeResource(resource);
            NotEnoughFood(foodConsumption - resource.Amount);
        }
    }

    public void NotEnoughFood(int deficit)
    {
        health *= notEnoughFoodHealthDecay;
    }

    public void UpdateCityHealth()
    {

    }
    #endregion


    #region Basics

    public void AddBuilding(DBuilding dBuilding)
    {
        if (buildings.ContainsKey(dBuilding.ID))
        {
            throw new BuildingAlreadyAddedException(string.Format("City '{0}' already has building '{1}'", name, dBuilding.Name));
        }
        else
        {
            buildings.Add(dBuilding.ID, dBuilding);

            if (dBuilding.Name == "Town Hall")
                townHall = dBuilding;
        }
    }

    public void AddPerson(DPerson dPerson)
    {
        if (people.ContainsKey(dPerson.ID))
        {
            throw new PersonAlreadyAddedException(string.Format("Person already added to city"));
        }
            people.Add(dPerson.ID, dPerson);
    }

    public void AddResource(DResource resource)
    {
        int amount = (int)(resource.Amount * SeasonResourceProduceMod(resource));
        AddResource(resource, amount);

        if (resource.Name.Equals("Shelter"))
            shelterResource = resources[resource.ID];
        else if (resource.Name.Equals("Fuel"))
            fuelResource = resources[resource.ID];
    }

    public void AddResource(DResource resource, int amount)
    {
        if (resources.ContainsKey(resource.ID))
        {
            resources[resource.ID].Amount += (int)(amount * SeasonResourceProduceMod(resource));
        }
        else
        {
            resources.Add(resource.ID, DResource.Create(resource, amount));
        }

        if (resource.Name.Equals("Shelter"))
            shelterResource = resources[resource.ID];
        else if (resource.Name.Equals("Fuel"))
            fuelResource = resources[resource.ID];
    }

    public void TakeResource(DResource resource)
    {
        if (resources.ContainsKey(resource.ID))
        {
            resources[resource.ID].Amount -= resource.Amount;
        }
        else
        {
            throw new ResourceNameNotFoundException(resource.Name);
        }
    }

    // todo - as resources are defined with constant names, include if checks here
    public float SeasonResourceProduceMod(DResource resource)
    {
        if (resource.Name == Constants.FOOD_RESOURCE_NAME)
            return DSeasons.modFoodProduction[(int)season];
        return 1f;
    }

    public void ConsumeResource(DResource resource, int amount)
    {
        if (resources.ContainsKey(resource.ID) && resources[resource.ID].Amount >= amount)
        {
           resources[resource.ID].Amount -= (int)(amount * SeasonResourceConsumedMod(resource));
        }
        else
        {
            throw new InsufficientResourceException(resource.ID.ToString());
        }
    }

    // todo - as resources are defined with constant names, include if checks here
    public float SeasonResourceConsumedMod(DResource resource)
    {
        if (resource.Name == Constants.FOOD_RESOURCE_NAME)
            return DSeasons.modFoodProduction[(int)season];
        return 1f;
    }

    public void ConsumeResource(DResource resource)
    {
        ConsumeResource(resource, resource.Amount);
    }

	public float CalculateExploration()
	{
		float countDiscovered = 0.0f;
		foreach(DBuilding dBuilding in buildings.Values)
		{
            if (dBuilding != townHall)
            {
                if (dBuilding.Status != DBuilding.DBuildingStatus.UNDISCOVERED)
				{
                    countDiscovered++;
                }
            }
		}

		if(countDiscovered > 0)
			return countDiscovered / (float)(buildings.Count - 1);
		else
			return countDiscovered;
	}
  public void CalculateResourceRates()
  {
    foreach (var entry in resources)
    {
      foreach (var entry0 in prevResources)
      {
        if (entry.Key == entry0.Key)
        {
          int change = entry.Value.Amount - entry0.Value.Amount;
          // Debug.Log(entry0.Value.Name +"[prev]: "+ entry0.Value.Amount);
          resourceRates[entry.Key] = DResource.Create(entry.Value,  change);
          break;
        }
      }

    }
    // foreach (var entry in resourceRates)
    // Debug.Log(entry.Value.Name +":  "+ entry.Value.Amount);
  }
    public DResource GetResource(string name)
    {
        int resourceID = DResource.NameToID(name);
        if (resources.ContainsKey(resourceID))
        {
            return resources[resourceID];
        }
        else
        {
            AddResource(DResource.Create(name));
            return resources[resourceID];
        }

    }

    public float PercentPopulationInfected()
    {
        float result = 0;
        foreach (KeyValuePair<int, DPerson> entry in people)
            if (entry.Value.Infection > 0)
                result++;
        result /= people.Count;
        return result;
    }

    public float DevelopedValue()
    {
        float explored = CalculateExploration();
        float assessed = PercentAssessed();
        float repaired = PercentRepaired();

        return (explored * Constants.CITY_DEVELOPMENT_PERCENT_FROM_EXPLORE) +
            (assessed * Constants.CITY_DEVELOPMENT_PERCENT_FROM_ASSESS) +
            (repaired * Constants.CITY_DEVELOPMENT_PERCENT_FROM_REPAIR);
    }

    public float PercentAssessed()
    {
        float result = 0f;
        foreach (KeyValuePair<int, DBuilding> entry in buildings)
            result += entry.Value.LevelAssessed;
        result /= buildings.Count;
        return result;
    }

    public float PercentRepaired()
    {
        float result = 0f;
        foreach (KeyValuePair<int, DBuilding> entry in buildings)
            result += (entry.Value.LevelDamaged + entry.Value.LevelInfectedRaw) / 2f;
        result /= buildings.Count;
        return result;
    }
    #endregion

    #region Map of Canada

    public void setEdges(List<string> s)
    {
      linkedCityKeys = s;
    }

    public void linkToCity(string cityKey)
    {
        if (!linkedCityKeys.Contains(cityKey))
            linkedCityKeys.Add(cityKey);
    }

    public IEnumerable<string> getAllLinkedCityKeys()
    {
        foreach (string key in linkedCityKeys)
            yield return key;
    }

    public bool isLinkedTo(string cityKey)
    {
        return linkedCityKeys.Contains(cityKey);
    }

    #endregion

    public void Explore(float exploreAmount)
    {
        explorationLevel = Mathf.Clamp01(explorationLevel + exploreAmount);

        float explorableBuildings = buildings.Count - 1.0f;
        float offsetPercentage = 1.0f / explorableBuildings;

        List<DBuilding> UnExploredBuildings = new List<DBuilding>();
        foreach(DBuilding dBuilding in buildings.Values)
        {
            if (dBuilding != townHall)
                if ((dBuilding.Status == DBuilding.DBuildingStatus.UNDISCOVERED))
                {
                    UnExploredBuildings.Add(dBuilding);
                }
        }
        if (explorationLevel - offsetPercentage * (explorableBuildings - UnExploredBuildings.Count) >= offsetPercentage)
        {
            int index = UnityEngine.Random.Range(0, UnExploredBuildings.Count - 1);
            UnExploredBuildings[index].Discover();
        }

    }

    public bool HasPeopleInTask(Type taskType)
    {
        if (taskType != typeof(DTask) && !taskType.IsSubclassOf(typeof(DTask)))
            return false;

        foreach (var entry in people)
            if (entry.Value.Task != null && entry.Value.Task.GetType() == typeof(DTask_Explore))
                return true;
        return false;
    }

    public int PeopleInTask(Type taskType)
    {
        if (taskType != typeof(DTask) && taskType.IsSubclassOf(typeof(DTask)))
            return 0;

        int result = 0;
        foreach (var entry in people)
            if (entry.Value.Task != null && entry.Value.Task.GetType() == typeof(DTask_Explore))
                result ++;
        return result;
    }

    #region Properties

    public Dictionary<int, DBuilding> Buildings
    {
        get { return buildings; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public Dictionary<int, DResource> ResourceRates
    {
      get { return resourceRates; }
      set { resourceRates = value; }
    }
    public Dictionary<int, DResource> Resources
    {
        get { return resources; }
        set { resources = value; }
    }

    public Dictionary<int, DPerson> People
    {
        get { return people; }
    }

    public List<string> LinkedCityKeys
    {
        get { return linkedCityKeys; }
        set { linkedCityKeys = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

	public float ExplorationLevel
	{
		get { return explorationLevel;}
	}

    public int Age
    {
        get { return age; }
    }

    public CityController CityController
    {
        get { return cityController; }
    }

    public DSeasons._season Season
    {
        get { return season; }
        set { season = value; }
    }

    public DateTime[] SeasonStartDates
    {
        get { return seasonStartDates; }
        set { seasonStartDates = value; }
    }

    public DateTime[] DeadOfWinterDates
    {
        get { return deadOfWinterStartEnd; }
        set { deadOfWinterStartEnd = value; }
    }

    public bool IsDeadOfWinter
    {
        get { return isDeadOfWinter; }
        set { isDeadOfWinter = value; }
    }

    public int ShelterTier
    {
        get { return shelterTier; }
    }

    public void RaiseShelterTier()
    {
        shelterTier = Mathf.Clamp(shelterTier + 1, 1, 5);
    }

    public void LowerShelterTier()
    {
        shelterTier = Mathf.Clamp(shelterTier - 1, 1, 5);
    }

    public int FuelToShelterConversion
    {
        get { return fuelToShelterConversion; }
    }

    public void RaiseFuelConversion()
    {
        int cap = 4 < fuelResource.Amount ? 4 : fuelResource.Amount;
        fuelToShelterConversion = Mathf.Clamp(fuelToShelterConversion + 1, 0, cap);
    }

    public void LowerFuelConversion()
    {
        int cap = 4 < fuelResource.Amount ? 4 : fuelResource.Amount;
        fuelToShelterConversion = Mathf.Clamp(fuelToShelterConversion - 1, 0, cap);
    }

    public DGame Game
    {
        get { return dGame; }
    }

    #endregion

    public JSONNode SaveToJSON()
    {
        JSONNode returnNode = new JSONObject();

        // Save general info about city
        returnNode.Add("name", new JSONString(name));
        returnNode.Add("age", new JSONNumber(age));
        returnNode.Add("explorationLevel", new JSONNumber(explorationLevel));

        // Save resource info
        returnNode.Add("shelterTier", new JSONNumber(shelterTier));
        returnNode.Add("fuelToShelterConversion", new JSONNumber(fuelToShelterConversion));

        // Save health and food stuff
        returnNode.Add("health", new JSONNumber(health));
        returnNode.Add("foodConsumption", new JSONNumber(foodConsumption));
        returnNode.Add("notEnoughFoodHealthDecay", new JSONNumber(notEnoughFoodHealthDecay));

        // Save season
        returnNode.Add("season", new JSONNumber((int)season));
        returnNode.Add("isDeadOfWinter", new JSONBool(isDeadOfWinter));

        #region Lists of Stuff

        // Save people
        JSONArray peopleList = new JSONArray();
        foreach (var person in people)
        {
            peopleList.Add(person.Value.SaveToJSON());
        }
        returnNode.Add("people", peopleList);

        // Save buildings
        JSONArray buildingList = new JSONArray();
        foreach (var building in buildings)
        {
            buildingList.Add(building.Value.SaveToJSON());
        }
        returnNode.Add("buildings", buildingList);

        // Save resources
        JSONArray resourceList = new JSONArray();
        foreach (var resource in resources)
        {
            resourceList.Add(resource.Value.SaveToJSON());
        }
        returnNode.Add("resources", resourceList);

        // Save linked cities
        JSONArray linkedCityList = new JSONArray();
        foreach (var city in linkedCityKeys)
        {
            linkedCityList.Add(new JSONString(city));
        }
        returnNode.Add("linked_cities", linkedCityList);

        #endregion

        return returnNode;
    }

    public static DCity LoadFromJSON(JSONNode jsonNode, DGame dGame, bool randomBuildingPlacement=false)
    {
        string _name = jsonNode["name"];


        // TODO: Store season start and end dates
        // Create city object
        DCity dCity = new DCity(_name, null, dGame.CurrentDate);
        dCity.cityController = dGame.GameController.CreateCityController(dCity);
        dCity.dGame = dGame;

        // Load general info about city
        dCity.age = RandJSON.JSONInt(jsonNode["age"]);
        dCity.explorationLevel = RandJSON.JSONFloat(jsonNode["explorationLevel"]);

        // Load resource info
        dCity.shelterTier = RandJSON.JSONInt(jsonNode["shelterTier"]);
        dCity.fuelToShelterConversion = RandJSON.JSONInt(jsonNode["fuelToShelterConversion"]);

        // TODO: Why are these variables static?
        // Load health and food stuff
        //dCity.health = RandJSON.JSONFloat(jsonNode["health"]);
        //dCity.foodConsumption = RandJSON.JSONInt(jsonNode["foodConsumption"]);
        //dCity.notEnoughFoodHealthDecay = RandJSON.JSONFloat(jsonNode["notEnoughFoodHealthDecay"]);

        // Load season
        DSeasons._season _season = (DSeasons._season)(RandJSON.JSONInt(jsonNode["season"]));
        bool _isDeadOfWinter = jsonNode["isDeadOfWinter"].AsBool;

        #region Lists of Stuff

        // Load people
        foreach (JSONNode person in jsonNode["people"].AsArray)
            DPerson.LoadFromJSON(person, dCity);

        // Load in all the possible locations for buildings
        List<Vector2> possibleBuildingLocations = null;
        if (randomBuildingPlacement)
        {
            possibleBuildingLocations = new List<Vector2>();
            foreach (JSONNode buildingLoc in jsonNode["building_locations"].AsArray)
            {
                // Get the random positions available
                float xPos = RandJSON.JSONFloat(buildingLoc["x"]);
                float yPos = RandJSON.JSONFloat(buildingLoc["y"]);

                possibleBuildingLocations.Add(new Vector2(xPos, yPos));
            }
        }

        // Load buildings
        foreach (JSONNode building in jsonNode["buildings"].AsArray)
        {
            DBuilding loadedBuilding = DBuilding.LoadFromJSON(building, dCity, randomBuildingPlacement);

            if (loadedBuilding.Name.Equals("Town Hall"))
            {
                dCity.townHall = loadedBuilding;
            }
            else if (randomBuildingPlacement)
            {
                // Pull a random building location from the list
                int randIndex = Mathf.RoundToInt(UnityEngine.Random.Range(0, possibleBuildingLocations.Count - 1));
                loadedBuilding.SetBuildingPosition(possibleBuildingLocations[randIndex]);
                possibleBuildingLocations.RemoveAt(randIndex);
            }
        }

        // Load resources
        foreach (JSONNode resource in jsonNode["resources"].AsArray)
            dCity.AddResource(DResource.LoadFromJSON(resource));

        // Load linked cities
        foreach (JSONNode linkedCity in jsonNode["linked_cities"].AsArray)
            dCity.linkToCity(linkedCity.Value);

        #endregion

        return dCity;
    }
}

#region Exceptions
public class InsufficientResourceException : Exception
{
    public InsufficientResourceException()
    {
    }

    public InsufficientResourceException(string message)
    : base(message)
    {
    }

    public InsufficientResourceException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

public class BuildingNotFoundException : Exception
{
    public BuildingNotFoundException()
    {
    }

    public BuildingNotFoundException(string message)
    : base(message)
    {
    }

    public BuildingNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

public class BuildingAlreadyAddedException : Exception
{
    public BuildingAlreadyAddedException()
    {
    }

    public BuildingAlreadyAddedException(string message) : base(message)
    {
    }

    public BuildingAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BuildingAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}

public class PersonNotFoundException : Exception
{
    public PersonNotFoundException()
    {
    }

    public PersonNotFoundException(string message)
    : base(message)
    {
    }

    public PersonNotFoundException(string message, Exception inner)
    : base(message, inner)
    {
    }
}

public class PersonAlreadyAddedException : Exception
{
    public PersonAlreadyAddedException()
    {
    }

    public PersonAlreadyAddedException(string message) : base(message)
    {
    }

    public PersonAlreadyAddedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected PersonAlreadyAddedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
#endregion
