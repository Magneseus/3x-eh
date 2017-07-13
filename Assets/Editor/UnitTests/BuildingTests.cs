using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public class BuildingTests
{
    private static string CITY_NAME = "Test City";
    private static string BUILDING_NAME = "Test Building";
    private static string RESOURCE_NAME = "Test Resource";
    private static int RESOURCE_START_AMOUNT = 3;

    private List<GameObject> mockObjects = new List<GameObject>();

    [TearDown]
    public void TearDown()
    {
        foreach(var entry in mockObjects)
            Object.DestroyImmediate(entry);
    }

    [Test]
    public void BuildingTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.City.Name, Is.EqualTo(city.Name));
        Assert.That(building.Tasks.Count, Is.EqualTo(0));
        Assert.That(building.Name, Is.EqualTo(BUILDING_NAME));
    }

    [Test]
    public void NameOverride()
    {
        var newName = "Test123";
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>())
        {
           Name = newName
        };

        Assert.That(building.Name, Is.EqualTo(newName));
    }

    #region Task Tests
    [Test]
    public void AddTask()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.Tasks.Count, Is.EqualTo(0));

        var task = new DTask(building, resource);

        Assert.That(building.Tasks.Count, Is.EqualTo(1));
        Assert.That(building.Tasks[task.ID].Output, Is.EqualTo(resource));
    }

    [Test]
    public void AddTaskTwice()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task = new DTask(building, resource);

        Assert.Throws<TaskAlreadyAddedException>(() =>
        {
            building.AddTask(task);
        });
    }

    [Test]
    public void PassesTaskOutputToCity()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));
    }

    [Test]
    public void DisablingTasks()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));

        task.DisableTask();
        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));
    }
    #endregion

    #region Status Tests
    [Test]
    public void BuildingStartsUndiscovered()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        Assert.That(building.Discovered, Is.False);
    }

    [Test]
    public void BuildingBecomesDiscovered()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        Assert.That(building.Discovered, Is.False);

        building.Assess(Constants.BUILDING_MAX_ASSESS / 2);
        Assert.That(building.Discovered, Is.True);
    }

    [Test]
    public void BuildingBecomesUndiscovered()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        Assert.That(building.Discovered, Is.False);

        building.Assess(Constants.BUILDING_MAX_ASSESS / 2);
        Assert.That(building.Discovered, Is.True);

        building.Assess((Constants.BUILDING_MAX_ASSESS / 2) * -1);
        Assert.That(building.Discovered, Is.False);
    }

    [Test]
    public void BuildingCanBeAssessed()
    {
        float increment = 0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MIN_ASSESS));

        building.Assess(increment);
        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MIN_ASSESS + increment));
    }

    [Test]
    public void BuildingCannotBeAssessedBelowMinimum()
    {
        float increment = -0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MIN_ASSESS));

        building.Assess(increment);
        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MIN_ASSESS));
    }

    [Test]
    public void BuildingCannotBeAssessedAboveMaximum()
    {
        float increment = Constants.BUILDING_MAX_ASSESS +  0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MIN_ASSESS));

        building.Assess(increment);
        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.BUILDING_MAX_ASSESS));
    }

    [Test]
    public void BuildingCanBeReclaimed()
    {
        float increment = 0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MIN_RECLAIM));
        building.Reclaim(increment);
        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MIN_RECLAIM + increment));
    }   

    [Test]
    public void BuildingCannotBeReclaimedBelowMinimum()
    {
        float increment = -0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MIN_RECLAIM));

        building.Reclaim(increment);
        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MIN_RECLAIM));
    }

    [Test]
    public void BuildingCannotBeReclaimedAboveMaximum()
    {
        float increment = Constants.BUILDING_MAX_RECLAIM + 0.2f;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());

        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MIN_RECLAIM));

        building.Reclaim(increment);
        Assert.That(building.LevelReclaimed, Is.EqualTo(Constants.BUILDING_MAX_RECLAIM));
    }  
    #endregion

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
