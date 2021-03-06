﻿using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;
using System;

// Disabling "Assigned to but not used" warnings
#pragma warning disable 0219

public class PersonTests
{
    private string CITY_NAME = "Test City";
		private string TOWN_HALL = "Town Hall";
    DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };


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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var person = new DPerson(city, Mock.Component<MeepleController>());

        Assert.That(person.City, Is.EqualTo(city));
        Assert.That(city.People[person.ID], Is.EqualTo(person));        
    }

    [Test]
    public void PersonSetAndRemoveTask()
    {
        var resource = DResource.Create("Test Resource", 1);
				var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
				var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());
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
    public void PersonSendToIdleTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>());
        Assert.That(person.Task, Is.Null);

        person.SetTask(task);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);

        person.MoveToTownHall();
        Assert.That(task.ContainsPerson(person), Is.False);
        Assert.That(townHall.getIdleTask().ContainsPerson(person), Is.True);
    }

    [Test]
    public void PersonSwitchTask()
    {
        var resource = DResource.Create("Test Resource", 1);
        var resource2 = DResource.Create("Test Resource 2", 2);

     	  var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
				var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());

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
		Assert.That(townHall.getIdleTask().ContainsPerson(person), Is.False);
    }

    [Test]
    public void PersonVerifyTaskNotFoundException()
    {
        var resource = DResource.Create("Test Resource", 1);

        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
		var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());

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

#pragma warning restore 0219