using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PersonTests
{
    private string CITY_NAME = "Test City";

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
        var person = new DPerson(city);

        Assert.That(person.City, Is.EqualTo(city));
        Assert.That(city.People[person.Id], Is.EqualTo(person));        
    }

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
