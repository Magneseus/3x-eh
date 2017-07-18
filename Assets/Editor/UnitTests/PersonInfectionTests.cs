using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;

public class PersonInfectionTests
{
    private static string CITY_NAME = "Test City";
    private static string BUILDING_NAME = "Test Building";
    private static string RESOURCE_NAME = "Test Resource";
    private static int RESOURCE_START_AMOUNT = 3;

    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
    }

    [Test]
    public void PersonInfectionTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void IncreaseInfection()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN
        };

        Assert.That(person.Infection, Is.EqualTo(Constants.MERSON_INFECTION_MIN));
        person.IncreaseInfection();
        Assert.That(person.Infection, Is.EqualTo(Constants.MERSON_INFECTION_MIN + 1));
    }

    [Test]
    public void DecreaseInfection()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MAX
        };

        person.DecreaseInfection();
        Assert.That(person.Infection, Is.LessThan(Constants.MERSON_INFECTION_MAX));
    }

    [Test]
    public void InfectionMaximum()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MAX
        };

        person.IncreaseInfection();        
        Assert.That(person.Infection, Is.EqualTo(Constants.MERSON_INFECTION_MAX));
    }

    [Test]
    public void InfectedMinimum()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN
        };

        person.DecreaseInfection();
        Assert.That(person.Infection, Is.EqualTo(Constants.MERSON_INFECTION_MIN));
    }

    [Test]
    public void LevelOneReducesTaskOutput()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN + 1
        };

        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        task.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.LessThan(task.Output.Amount));
    }

    [Test]
    public void LevelOneReduceTaskRepair()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN + 1
        };

        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        task.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.LessThan(task.Output.Amount));
    }




}