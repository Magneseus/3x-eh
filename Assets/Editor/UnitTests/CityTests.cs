using NUnit.Framework;
using System.Collections.Generic;
using Assets.Editor.UnitTests;
using System;

// Disabling "Assigned to but not used" warnings
#pragma warning disable 0219

public class CityTests
{
    private string CITY_NAME = "Test City";
    private string LINKED_CITY_NAME = "Linked City";
	private string TOWN_HALL = "Town Hall";
    private string BUILDING_NAME = "Test Building";
    private string BUILDING_NAME_2 = "Other Test Building";
    private DateTime[] defaultSeasonStartDates = {new DateTime(2017,4,1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1)};

    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
    }

    [Test]
    public void CityTestsSimplePasses()
    {
        Assert.Pass();
    }

    #region Basic Methods
    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        Assert.That(city.Buildings, Is.Not.Null);
        Assert.That(city.Resources, Is.Not.Null);
        Assert.That(city.People, Is.Not.Null);
        Assert.That(city.Name, Is.EqualTo(CITY_NAME));
        Assert.That(city.Age, Is.EqualTo(0));
    }

    [Test]
    public void Name()
    {
        var newName = "Test123";
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now)
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuilding()
    {
        var startingBuildingCount = 0;
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

		var building = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());
		Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 1));
		Assert.That(building.City, Is.EqualTo(city));

        var building1 = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 2));
        Assert.That(building1.City, Is.EqualTo(city));


        var building2 = new DBuilding(city, BUILDING_NAME_2, Mock.Component<BuildingController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 3));
        Assert.That(building2.City, Is.EqualTo(city));
    }    

    [Test]
    public void AddSameBuildingTwice()
    {
        var startingBuildingCount = 0;
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 1));
        Assert.That(building.City, Is.EqualTo(city));

        Assert.Throws<BuildingAlreadyAddedException>(() =>
        {
            city.AddBuilding(building);
        });
    }

    [Test]
    public void AddResource()
    {
        var resource = DResource.Create("Test", 0);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.AddResource(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void CountPeople()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        Assert.That(city.People.Count, Is.EqualTo(0));

        var person = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(1));
        Assert.That(person.City, Is.EqualTo(city));
    }
	[Test]
	public void ExplorationLevel()
	{
		var startLevelExploration = 0.0f;
		var numberOfDaysPassed = 7;
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());
		var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var person = new DPerson(city, Mock.Component<MeepleController>());

        Assert.That(city.ExplorationLevel, Is.EqualTo(startLevelExploration));
        townHall.getExploreTask().AddPerson(person);
        for(int i=0;i<10;i++)
		    city.TurnUpdate(numberOfDaysPassed);

		Assert.That(city.ExplorationLevel, Is.EqualTo(1.0f));
	}
    
    [Test]
    public void AddPerson()
    {
        var startCount = 0;
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);

        Assert.That(city.People.Count, Is.EqualTo(startCount));        
        
        var person = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 1));
        Assert.That(person.City, Is.EqualTo(city));

        var otherPerson = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 2));
        Assert.That(otherPerson.City, Is.EqualTo(city));
    }

    [Test]
    public void AddSamePersonTwice()
    {
        var startCount = 0;
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);

        Assert.That(city.People.Count, Is.EqualTo(startCount));

        var person = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 1));
        Assert.That(person.City, Is.EqualTo(city));

        Assert.Throws<PersonAlreadyAddedException>(() =>
        {
            city.AddPerson(person);
        });
    }

    [Test]
    public void ChangePersonToValidTask()
    {
        string RESOURCE_NAME = "Test Resource";
        int RESOURCE_A_AMOUNT = 1;
        int RESOURCE_B_AMOUNT = 1000;

        var resource_A = DResource.Create(RESOURCE_NAME, RESOURCE_A_AMOUNT);
        var resource_B = DResource.Create(RESOURCE_NAME, RESOURCE_B_AMOUNT);

        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task_A = Mock.CleanTask(building, resource_A);
        var task_B = Mock.CleanTask(building, resource_B);

        var person = new DPerson(city, Mock.Component<MeepleController>());

        Assert.That(city.People[person.ID], Is.EqualTo(person));
        Assert.That(person.City, Is.EqualTo(city));
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        person.SetTask(task_A);
        city.TurnUpdate(1);
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_A_AMOUNT));

        person.SetTask(task_B);
        city.TurnUpdate(1);
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_A_AMOUNT + RESOURCE_B_AMOUNT));
    }

    [Test]
    public void PercentAssessed()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building1 = new DBuilding(city, BUILDING_NAME+1, Mock.Component<BuildingController>());
        var building2 = new DBuilding(city, BUILDING_NAME+2, Mock.Component<BuildingController>());

        float val1 = 0.5f;
        float val2 = 0f;
        building1.LevelAssessed = val1;
        building2.LevelAssessed = val2;

        float expected = (val1 + val2) / 2f;
        Assert.That(city.PercentCityAssessed(), Is.EqualTo(expected));
    }
    #endregion

    #region Update Methods
    [Test]
    public void TurnUpdateDaysPassed()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 7;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);

        for (var i = 0; i < 10; i++)
        {
            Assert.That(city.Age, Is.EqualTo(i * numberOfDaysPassed));
            city.TurnUpdate(numberOfDaysPassed);
        }
    }

    [Test]
    public void TurnUpdateSufficientResourcesSingleTurn()
    {
        var resourceName = "Test";
        var outputAmount = 2;
        var zero = 0;
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = Mock.CleanTask(building, output);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(zero));
        Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount));
    }

    [Test]
    public void TurnUpdateSufficientResourcesMultipleTurns()
    {
        var numberOfTurns = 5;
        var resourceName = "Test";
        var outputAmount = 2;
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = Mock.CleanTask(building, output);

        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);

        for (var i=0; i<numberOfTurns; i++)
        {
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * i));
            Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * (i+1)));
        }
    }

    [Test]
    public void FoodPassivelyConsumed()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 7;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);
        int numFood = 50;
        city.AddResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), numFood);
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(numFood));
        int newFood = city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount;

        for (var i = 0; i < 10; i++)
        {
            city.TurnUpdate(numberOfDaysPassed);
            newFood = city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount;
            Assert.That(newFood, Is.LessThan(numFood));
            numFood = newFood;
        }
    }

    [Test]
    public void FoodDeficitLowersHealth()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 7;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);
        float health = 0.5f;
        city.Health = health;

        city.TurnUpdate(numberOfDaysPassed);
        Assert.That(city.Health, Is.LessThan(health));
    }
    #endregion

    #region Linked Cities
    [Test]
    public void ConstructorLinksCities()
    {
        List<string> linkedCities = new List<string>();
        linkedCities.Add(LINKED_CITY_NAME);

        DCity city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now, linkedCities);
        Assert.That(city.LinkedCityKeys, Is.EqualTo(linkedCities));

        DCity unlinkedCity = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        Assert.IsEmpty(unlinkedCity.LinkedCityKeys);
    }

    [Test]
    public void AddsLinkedCity()
    {
        DCity city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        city.linkToCity(LINKED_CITY_NAME);
        Assert.True(city.LinkedCityKeys.Contains(LINKED_CITY_NAME));
    }

    [Test]
    public void ReturnsAllLinkedCities()
    {
        int numCities = 4;

        List<string> linkedCities = new List<string>();

        for(int i = 0; i < numCities; i++)
            linkedCities.Add(LINKED_CITY_NAME + i.ToString());

        DCity city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now, linkedCities);

        foreach(string cityKey in city.getAllLinkedCityKeys())
        {
            Assert.True(linkedCities.Contains(cityKey));
            linkedCities.Remove(cityKey);
        }
        Assert.True(linkedCities.Count == 0);
    }
    #endregion

    #region Seasons

    [Test]
    public void DeadOfWinter()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 3;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);


        DateTime date = city.DeadOfWinterDates[0].AddDays(-1);
        city.TurnUpdate(numberOfDaysPassed);
        date = date.AddDays(numberOfDaysPassed);
        city.UpdateSeason(date);

        Assert.IsTrue(city.IsDeadOfWinter);

        date = city.DeadOfWinterDates[1].AddDays(-1);
        city.TurnUpdate(numberOfDaysPassed);
        date = date.AddDays(numberOfDaysPassed);
        city.UpdateSeason(date);

        Assert.IsFalse(city.IsDeadOfWinter);
    }


    [Test]
    public void SeasonsAffectFoodProduction()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);

        int food = 5;
        city.Season = DSeasons._season.WINTER;
        city.AddResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), food);
        int expectedAmount = (int)(food * city.SeasonResourceProduceMod(city.GetResource(Constants.FOOD_RESOURCE_NAME)));
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(expectedAmount));
    }

    [Test]
    public void SeasonsAffectFoodConsumption()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 7;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);
        int numFood = 50;
        city.AddResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), numFood);
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(numFood));
        city.Season = DSeasons._season.WINTER;

        int baseConsume = 10;

        city.ConsumeResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), baseConsume);
        int expected = numFood - (int)(baseConsume * city.SeasonResourceConsumedMod(city.GetResource(Constants.FOOD_RESOURCE_NAME)));
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(expected));
    } 
    #endregion
}

#pragma warning restore 0219