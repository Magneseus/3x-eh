using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public class GameManagerTests
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
    public void GameManagerTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        DGame gm = new DGame();
        Assert.That(gm.Cities, Is.Not.Null);
        Assert.That(gm.TurnDuration, Is.EqualTo(7));
        Assert.That(gm.TurnNumber, Is.EqualTo(0));
        Assert.That(gm.DaysTranspired, Is.EqualTo(0));


    }
    [Test]
    public void CityTurnUpdateTest()
    {
        var gm = new DGame();
        for (var i = 0; i < 10; i++)
        {
            Assert.That(gm.TurnNumber, Is.EqualTo(i));
            Assert.That(gm.DaysTranspired, Is.EqualTo(i*gm.TurnDuration));
            gm.EndTurnUpdate();
        }

    }

    [Test]
    public void AddCityTest()
    {
        var game = new DGame();
        var city = new DCity(CITY_NAME, MockCityController());
        
        Assert.That(game.Cities.Count, Is.EqualTo(0));

        game.AddCity(city);
        Assert.That(game.Cities.Count, Is.EqualTo(1));
    }

    [Test]
    public void MovePersonTest()
    {
        var game = new DGame();
        var city = new DCity(CITY_NAME, MockCityController());
        game.AddCity(city);

        var building1 = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        var building2 = new DBuilding(city, BUILDING_NAME_2, MockBuildingController());
        var person = new DPerson(building1);

        game.MovePerson(person, building2);

        Assert.That(building1.Population.Contains(person), Is.False);
        Assert.That(building2.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building2));
                
        game.MovePerson(person, building1);

        Assert.That(building1.Population.Contains(person), Is.True);
        Assert.That(building2.Population.Contains(person), Is.False);
        Assert.That(person.Building, Is.EqualTo(building1));
    }

    private BuildingController MockBuildingController()
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<BuildingController>().GetComponent<BuildingController>();
    }

    private CityController MockCityController()
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<CityController>().GetComponent<CityController>();
    }
}
