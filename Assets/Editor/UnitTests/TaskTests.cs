using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TaskTests
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
    public void TaskTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource, 3, "test name");

        Assert.That(task.Name, Is.EqualTo("test name"));
        Assert.That(task.MaxPeople, Is.EqualTo(3));
        Assert.That(task.Output, Is.EqualTo(resource));
        Assert.That(task.ListOfPeople, Is.Not.Null);
        Assert.That(task.Building, Is.EqualTo(building));
        Assert.That(task.Enabled, Is.True);

        Assert.That(building.Tasks[task.ID], Is.EqualTo(task));
    }

    [Test]
    public void TaskAddAndRemovePerson()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock<MeepleController>());

        Assert.That(person.Task, Is.Null);

        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ListOfPeople.Contains(person), Is.True);

        task.RemovePerson(person);
        Assert.That(person.Task, Is.Null);
        Assert.That(task.ListOfPeople.Contains(person), Is.False);
    }

    [Test]
    public void TaskVerifyPersonNotFoundException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock<MeepleController>());
        var person2 = new DPerson(city, Mock<MeepleController>());

        task.AddPerson(person);
        task.RemovePerson(person);

        Assert.Throws<PersonNotFoundException>(() =>
        {
            task.RemovePerson(person);
        });

        Assert.Throws<PersonNotFoundException>(() =>
        {
            task.RemovePerson(person2);
        });
    }

    [Test]
    public void TaskVerifyPersonAlreadyAddedException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock<MeepleController>());

        task.AddPerson(person);

        Assert.Throws<PersonAlreadyAddedException>(() =>
        {
            task.AddPerson(person);
        });
    }

    [Test]
    public void TaskVerifyTaskFullException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock<CityController>());
        var building = new DBuilding(city, "Test Building", Mock<BuildingController>());
        var task = new DTask(building, resource, 1, "temp");
        var person = new DPerson(city, Mock<MeepleController>());
        var person2 = new DPerson(city, Mock<MeepleController>());

        task.AddPerson(person);

        Assert.Throws<TaskFullException>(() =>
        {
            task.AddPerson(person2);
        });
    }

    private T Mock<T>() where T : Component
    {
        var mockObj = new GameObject();
        mockObjects.Add(mockObj);
        return mockObj.AddComponent<T>().GetComponent<T>();
    }
}
