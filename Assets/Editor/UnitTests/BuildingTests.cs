﻿using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;
using System;

public class BuildingTests
{
    private static string CITY_NAME = "Test City";
    private static string BUILDING_NAME = "Test Building";
    private static string RESOURCE_NAME = "Test Resource";
    private static int RESOURCE_START_AMOUNT = 3;
    DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };

    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
    }

    [Test]
    public void BuildingTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());

        Assert.That(building.City.Name, Is.EqualTo(city.Name));
        Assert.That(building.Tasks.Count, Is.EqualTo(1));
        Assert.That(building.Name, Is.EqualTo(BUILDING_NAME));
    }

    [Test]
    public void NameOverride()
    {
        var newName = "Test123";
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>())
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());

        Assert.That(building.Tasks.Count, Is.EqualTo(1));

        var task = new DTask(building, resource);

        Assert.That(building.Tasks.Count, Is.EqualTo(2));
        Assert.That(building.Tasks[task.ID].Output, Is.EqualTo(resource));
    }

    [Test]
    public void AddTaskTwice()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = Mock.CleanTask(building, resource);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));
    }

    [Test]
    public void DisablingTasks()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = Mock.CleanTask(building, resource);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));

        task.DisableTask();
        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));
    }
    #endregion

    #region Damage Tests
    [Test]
    public void Damaged()
    {

    }

    [Test]
    public void Infected()
    {

    }

    [Test]
    public void LevelDamaged()
    {

    }

    [Test]
    public void LevelInfected()
    {

    }
    #endregion
}
