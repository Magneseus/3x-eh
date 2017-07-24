using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;
using Assets.Editor.UnitTests;

// Disabling "Assigned to but not used" warnings
#pragma warning disable 0219

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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MAX
        };
        person.IncreaseInfection();
        Assert.That(person.InfectionActual, Is.EqualTo(Constants.MERSON_INFECTION_MAX));
    }

    [Test]
    public void InfectedMinimum()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
    public void LevelOneReduceTaskFungalRepair()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
        
        var taskSlot = task.GetTaskSlot(0);
        taskSlot.LevelInfected = Constants.TASK_MAX_FUNGAL_DMG;
        task.TurnUpdate(1);

        Assert.That(taskSlot.LevelInfected, Is.EqualTo(Constants.TASK_MAX_FUNGAL_DMG - Constants.TEMP_REPAIR_AMOUNT * Constants.MERSON_INFECTION_TASK_MODIFIER));
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));
    }

    [Test]
    public void LevelOneReduceTaskStructureRepair()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
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
        
        var taskSlot = task.GetTaskSlot(0);
        taskSlot.LevelInfected = Constants.TASK_MIN_FUNGAL_DMG; 
        taskSlot.LevelDamaged = Constants.TASK_MAX_STRUCTURAL_DMG;
        task.TurnUpdate(1);

        Assert.That(taskSlot.LevelDamaged, Is.EqualTo(Constants.TASK_MAX_STRUCTURAL_DMG - Constants.TEMP_REPAIR_AMOUNT * Constants.MERSON_INFECTION_TASK_MODIFIER));
        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));
    }

    [Test]
    public void LevelOneReduceTaskAssess()
    {
        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = new DTask_Assess(building);

        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN + 1
        };

        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);
        Assert.That(task.AssessAmount, Is.EqualTo(Constants.DEFAULT_ASSESS_AMOUNT));
        Assert.That(building.Assessed, Is.False);
        Assert.That(building.LevelAssessed, Is.EqualTo(0));

        task.TurnUpdate(1);

        Assert.That(building.LevelAssessed, Is.EqualTo(Constants.DEFAULT_ASSESS_AMOUNT * Constants.MERSON_INFECTION_TASK_MODIFIER));        
    }
    public void TreatTaskRandomlyReducesInfectionOfAPerson()
    {
        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), Constants.DEFAULT_SEASON_DATES, Constants.DEFAULT_DATE);
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = new DTask(building, null, 1, "Treat People",0.0f);

        var person = new DPerson(city, Mock.Component<MeepleController>())
        {
            Infection = Constants.MERSON_INFECTION_MIN + 1
        };


        building.Assess(1.0f);
        task.ForceClean();
        task.ForceFixed();
        task.AddPerson(person);
        Assert.That(person.Task, Is.EqualTo(task));
        Assert.That(task.ContainsPerson(person), Is.True);
        Assert.That(person.Infection, Is.EqualTo(1));

        task.TurnUpdate(1);

        Assert.That(person.Infection, Is.EqualTo(0));

    }
}

#pragma warning restore 0219