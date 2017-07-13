using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public class GameManagerTests
{
    private List<GameObject> mockObjects = new List<GameObject>();

    private string CITY_NAME = "Test City";
    private string LINKED_CITY_NAME = "Linked City";

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
        var city = new DCity(CITY_NAME, Mock<CityController>());
        
        Assert.That(game.Cities.Count, Is.EqualTo(0));

        game.AddCity(city);
        Assert.That(game.Cities.Count, Is.EqualTo(1));
    }    

    [Test]
    public void LinkCities()
    {
        var game = new DGame();
        DCity city = new DCity(CITY_NAME, Mock<CityController>());
        DCity linkedCity = new DCity(LINKED_CITY_NAME, Mock<CityController>());

        game.AddCity(city);
        game.AddCity(linkedCity);
        game.LinkCities(CITY_NAME, LINKED_CITY_NAME);

        Assert.True(city.isLinkedTo(LINKED_CITY_NAME));
        Assert.True(linkedCity.isLinkedTo(CITY_NAME));
    }

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
