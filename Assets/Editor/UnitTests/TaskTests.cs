using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;
using System;

public class TaskTests
{
    private string CITY_NAME = "Test City";
    DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };

    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 3, "test name", 0.0f);

        Assert.That(task.Name, Is.EqualTo("test name"));
        Assert.That(task.Output, Is.EqualTo(resource));
        Assert.That(task.Building, Is.EqualTo(building));
        Assert.That(task.Enabled, Is.True);
        
        Assert.That(task.MaxPeople, Is.EqualTo(3));

        Assert.That(task.LevelDamaged, Is.Not.Null);
        Assert.That(task.Damaged, Is.Not.Null);

        Assert.That(task.LevelInfected, Is.Not.Null);
        Assert.That(task.Infected, Is.Not.Null);


        Assert.That(building.Tasks[task.ID], Is.EqualTo(task));
    }

    [Test]
    public void TaskAddAndRemovePerson()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);

        var person = new DPerson(city, Mock.Component<MeepleController>());

        Assert.That(person.Task, Is.Null);

        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);

        task.RemovePerson(person);
        Assert.That(person.Task, Is.Null);
        Assert.That(task.ContainsPerson(person), Is.False);
    }

    [Test]
    public void TaskVerifyPersonNotFoundException()
    {
        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        var person2 = new DPerson(city, Mock.Component<MeepleController>());

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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource);
        var person = new DPerson(city, Mock.Component<MeepleController>());

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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "temp", 0.0f);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        var person2 = new DPerson(city, Mock.Component<MeepleController>());

        task.AddPerson(person);

        Assert.Throws<TaskFullException>(() =>
        {
            task.AddPerson(person2);
        });
    }

    [Test]
    public void DamageCanBeRepaired()
    {
        var amount = Constants.TASK_MAX_STRUCTURAL_DMG / 2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MIN_FUNGAL_DMG;
        taskSlot.LevelDamaged = Constants.TASK_MAX_STRUCTURAL_DMG;
        taskSlot.Repair(amount);
        Assert.That(task.LevelDamaged, Is.EqualTo(Constants.TASK_MAX_STRUCTURAL_DMG - amount));
    }

    [Test]
    public void InfectionCanBeRepaired()
    {
        var amount = Constants.TASK_MAX_FUNGAL_DMG / 2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MAX_FUNGAL_DMG;
        taskSlot.Repair(amount);
        Assert.That(task.LevelInfected, Is.EqualTo(Constants.TASK_MAX_FUNGAL_DMG - amount));
    }

    [Test]
    public void InfectionRepairedBeforeDamage()
    {
        var amount = Constants.TASK_MAX_FUNGAL_DMG / 2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelDamaged = Constants.TASK_MAX_STRUCTURAL_DMG;
        taskSlot.LevelInfected = Constants.TASK_MAX_FUNGAL_DMG;

        taskSlot.Repair(amount);

        Assert.That(task.LevelDamaged, Is.EqualTo(Constants.TASK_MAX_STRUCTURAL_DMG));
        Assert.That(task.LevelInfected, Is.EqualTo(Constants.TASK_MAX_FUNGAL_DMG - amount));
    }


    [Test]
    public void DamageIsClampedMax()
    {
        var amount = Constants.TASK_MAX_STRUCTURAL_DMG * -2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MIN_FUNGAL_DMG;
        taskSlot.LevelDamaged = Constants.TASK_MAX_STRUCTURAL_DMG / 2;
        taskSlot.Repair(amount);
        Assert.That(task.LevelDamaged, Is.EqualTo(Constants.TASK_MAX_STRUCTURAL_DMG));
    }
    [Test]
    public void DamageIsClampedMin()
    {
        var amount = Constants.TASK_MAX_STRUCTURAL_DMG * 2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MIN_FUNGAL_DMG;
        taskSlot.LevelDamaged = Constants.TASK_MAX_STRUCTURAL_DMG;
        taskSlot.Repair(amount);
        Assert.That(task.LevelDamaged, Is.EqualTo(Constants.TASK_MIN_STRUCTURAL_DMG));
    }

    [Test]
    public void InfectionIsClampedMax()
    {
        var amount = Constants.TASK_MAX_FUNGAL_DMG * -2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MAX_FUNGAL_DMG / 2;
        taskSlot.Repair(amount);
        Assert.That(task.LevelInfected, Is.EqualTo(Constants.TASK_MAX_FUNGAL_DMG));
    }
    [Test]
    public void InfectionIsClampedMin()
    {
        var amount = Constants.TASK_MAX_FUNGAL_DMG * 2;

        var resource = DResource.Create("Test Resource", 1);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var building = new DBuilding(city, "Test Building", Mock.Component<BuildingController>());
        var task = new DTask(building, resource, 1, "test", 0.0f);
        var taskSlot = task.GetTaskSlot(0);

        taskSlot.LevelInfected = Constants.TASK_MAX_FUNGAL_DMG;
        taskSlot.Repair(amount);
        Assert.That(task.LevelInfected, Is.EqualTo(Constants.TASK_MIN_FUNGAL_DMG));
    }
}
