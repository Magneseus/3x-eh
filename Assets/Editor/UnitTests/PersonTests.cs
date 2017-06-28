using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PersonTests
{
    private string CITY_NAME = "Test City";
    private string BUILDING_NAME = "Test Building";

    private List<GameObject> mockObjects = new List<GameObject>();

    [TearDown]
    public void TearDown()
    {
        foreach (var entry in mockObjects)
            Object.DestroyImmediate(entry);
    }

    [Test]
    public void PersonTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock<BuildingController>());
        var person = new DPerson(building);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Contains(person), Is.True);        
    }

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
