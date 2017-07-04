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
        var person = new DPerson(city, Mock<MeepleController>());

        Assert.That(person.City, Is.EqualTo(city));
        Assert.That(city.People[person.ID], Is.EqualTo(person));        
    }

    [Test]
    public void PersonSetAndRemoveTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock<MeepleController>());
        Assert.That(person.Task, Is.Null);

        person.SetTask(task);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ListOfPeople.Contains(person), Is.True);

        person.RemoveTask(task);
        Assert.That(person.Task, Is.Null);
        Assert.That(task.ListOfPeople.Contains(person), Is.False);
    }

    [Test]
    public void PersonSwitchTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var resource2 = DResource.Create("Test Resource 2", 2);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);
        var task2 = new DTask(building, resource2);

        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ListOfPeople.Contains(person), Is.True);

        person.SetTask(task2);
        Assert.That(person.Task, Is.EqualTo(task2));
        Assert.That(task2.ListOfPeople.Contains(person), Is.True);
        Assert.That(task.ListOfPeople.Contains(person), Is.False);
    }

    [Test]
    public void PersonVerifyTaskNotFoundException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock<MeepleController>());
        person.SetTask(task);
        person.RemoveTask(task);

        Assert.Throws<TaskNotFoundException>(() =>
        {
            person.RemoveTask(task);
        });
    }

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
