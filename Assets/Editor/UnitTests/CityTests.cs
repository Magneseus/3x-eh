using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CityTests
{
    private List<GameObject> mockObjects = new List<GameObject>();

    private string CITY_NAME = "Test City";
    private string BUILDING_NAME = "Test Building";
    private string BUILDING_NAME_2 = "Other Test Building";

    [TearDown]
    public void TearDown()
    {
        foreach (var entry in mockObjects)
            Object.DestroyImmediate(entry);
    }

    [Test]
    public void CityTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
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
        var city = new DCity(CITY_NAME, Mock<CityController>())
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuilding()
    {
        var startingBuildingCount = 0;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 1));
        Assert.That(building.City, Is.EqualTo(city));

        var building2 = new DBuilding(city, BUILDING_NAME_2, Mock<BuildingController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 2));
        Assert.That(building2.City, Is.EqualTo(city));
    }    

    [Test]
    public void AddSameBuildingTwice()
    {
        var startingBuildingCount = 0;
        var city = new DCity(CITY_NAME, Mock<CityController>());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
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
        var city = new DCity(CITY_NAME, Mock<CityController>());

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.AddResource(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void CountPeople()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        Assert.That(city.People.Count, Is.EqualTo(0));

        var person = new DPerson(city, Mock<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(1));
        Assert.That(person.City, Is.EqualTo(city));
    }
    
    [Test]
    public void AddPerson()
    {
        var startCount = 0;
        var city = new DCity(CITY_NAME, Mock<CityController>());

        Assert.That(city.People.Count, Is.EqualTo(startCount));        
        
        var person = new DPerson(city, Mock<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 1));
        Assert.That(person.City, Is.EqualTo(city));

        var otherPerson = new DPerson(city, Mock<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 2));
        Assert.That(otherPerson.City, Is.EqualTo(city));
    }

    [Test]
    public void AddSamePersonTwice()
    {
        var startCount = 0;
        var city = new DCity(CITY_NAME, Mock<CityController>());

        Assert.That(city.People.Count, Is.EqualTo(startCount));

        var person = new DPerson(city, Mock<MeepleController>());
        Assert.That(city.People.Count, Is.EqualTo(startCount + 1));
        Assert.That(person.City, Is.EqualTo(city));

        Assert.Throws<PersonAlreadyAddedException>(() =>
        {
            city.AddPerson(person);
        });
    }

    [Test]
    public void TurnUpdateDaysPassed()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var numberOfDaysPassed = 7;

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

        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task = new DTask(building, output);
        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);

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

        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task = new DTask(building, output);
        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);

        for (var i=0; i<numberOfTurns; i++)
        {
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * i));
            Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * (i+1)));
        }
    }   

    [Test]
    public void ChangePersonToValidTask()
    {
        string RESOURCE_NAME = "Test Resource";
        int RESOURCE_A_AMOUNT = 1;
        int RESOURCE_B_AMOUNT = 1000;

        var resource_A = DResource.Create(RESOURCE_NAME, RESOURCE_A_AMOUNT);
        var resource_B = DResource.Create(RESOURCE_NAME, RESOURCE_B_AMOUNT);

        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var task_A = new DTask(building, resource_A);
        var task_B = new DTask(building, resource_B);

        var person = new DPerson(city, Mock<MeepleController>());

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
    
    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}