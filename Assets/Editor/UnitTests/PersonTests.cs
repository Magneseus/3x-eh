using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;

public class PersonTests
{
    private string CITY_NAME = "Test City";

    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
    }

    [Test]
    public void PersonTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var person = new DPerson(city, Mock.Component<MeepleController>());

        Assert.That(person.City, Is.EqualTo(city));
        Assert.That(city.People[person.ID], Is.EqualTo(person));        
    }

    [Test]
    public void PersonSetAndRemoveTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(person.Task, Is.Null);

        person.SetTask(task);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);

        person.RemoveTask();
        Assert.That(person.Task, Is.Null);
        Assert.That(task.ContainsPerson(person), Is.False);
    }

    [Test]
    public void PersonSwitchTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var resource2 = DResource.Create("Test Resource 2", 2);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);
        var task2 = new DTask(building, resource2);

        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);

        person.SetTask(task2);
        Assert.That(person.Task, Is.EqualTo(task2));
        Assert.That(task2.ContainsPerson(person), Is.True);
        Assert.That(task.ContainsPerson(person), Is.False);
    }

    [Test]
    public void PersonVerifyTaskNotFoundException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);
        person.RemoveTask();

        Assert.Throws<TaskNotFoundException>(() =>
        {
            person.RemoveTask();
        });
    }    
}
