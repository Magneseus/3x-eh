using System;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;

public class CityTests
{
    private string CITY_NAME = "Test City";
    private string CITY_NAME_2 = " Other Test City";
    private string BUILDING_NAME = "Test Building";
    private string BUILDING_NAME_2 = "Other Test Building";
    private string BUILDING_NAME_3 = "Other Other Test Building";

    [Test]
    public void CityTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        Assert.That(city.Buildings, Is.Not.Null);
        Assert.That(city.Resources, Is.Not.Null);
        Assert.That(city.People, Is.Not.Null);
        Assert.That(city.Name, Is.EqualTo(CITY_NAME));
        Assert.That(city.Age, Is.EqualTo(0));
    }

    [Test]
    public void Name()
    {
        var newName = "Test123";
        var city = new DCity(CITY_NAME, MockCityController())
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuilding()
    {
        var startingBuildingCount = 0;
        var city = new DCity(CITY_NAME, MockCityController());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 1));
        Assert.That(building.City, Is.EqualTo(city));

        var building2 = new DBuilding(city, BUILDING_NAME_2, MockBuildingController());
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 2));
        Assert.That(building2.City, Is.EqualTo(city));
    }

    [Test]
    public void AddResource()
    {
        var resource = DResource.Create("Test", 0);
        var city = new DCity(CITY_NAME, MockCityController());

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.AddResource(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void CountPeople()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        Assert.That(city.People.Count, Is.EqualTo(0));

        var person = new DPerson(building);
        Assert.That(city.People.Count, Is.EqualTo(1));
        Assert.That(person.Building, Is.EqualTo(building));
    }
    
    [Test]
    public void AddPopulation()
    {
        var startCount = 0;
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        Assert.That(city.People.Count, Is.EqualTo(startCount));        
        
        var person = new DPerson(building);
        Assert.That(city.People.Count, Is.EqualTo(startCount + 1));
        Assert.That(person.Building, Is.EqualTo(building));

        var otherPerson = new DPerson(building);
        Assert.That(city.People.Count, Is.EqualTo(startCount + 2));
        Assert.That(otherPerson.Building, Is.EqualTo(building));
    }

    [Test]
    public void TurnUpdateDaysPassed()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var numberOfDaysPassed = 7;

        for (var i = 0; i < 10; i++)
        {
            Assert.That(city.Age, Is.EqualTo(i * numberOfDaysPassed));
            city.TurnUpdate(numberOfDaysPassed);
        }
    }

    [Test]
    public void TurnUpdateSufficientResourcesSingleTurn()
    {
        var resourceName = "Test";
        var outputAmount = 2;
        var zero = 0;
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        building.AddResourceOutput(output);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(zero));
        Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount));
    }

    [Test]
    public void TurnUpdateSufficientResourcesMultipleTurns()
    {
        var numberOfTurns = 5;
        var resourceName = "Test";
        var outputAmount = 2;
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        building.AddResourceOutput(output);
        
        for(var i=0; i<numberOfTurns; i++)
        {
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * i));
            Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount * (i+1)));
        }
    }

    [Test]
    public void TurnUpdateInsufficientResourcesSingleTurn()
    {
        var resourceName = "Test";

        var consumeAmount = 3;
        var consume = DResource.Create(resourceName, consumeAmount);

        var stockpileAmount = 2;
        var stockpile = DResource.Create(resourceName, stockpileAmount);


        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        building.AddResourceConsumption(consume);
        city.AddResource(stockpile);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount));

        var missingResourceId = Assert.Throws<InsufficientResourceException>(() =>
        {
            city.TurnUpdate(1);
        });

        Assert.That(int.Parse(missingResourceId.Message), Is.EqualTo(DResource.NameToId(resourceName)));        
    }

    [Test]
    public void TurnUpdateInsufficientResourcesAfterMultipleTurns()
    {
        var numberOfValidTurns = 4;
        var resourceName = "Test";

        var consumeAmount = 3;
        var consume = DResource.Create(resourceName, consumeAmount);

        var stockpileAmount = numberOfValidTurns * consumeAmount + 1;
        var stockpile = DResource.Create(resourceName, stockpileAmount);


        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        building.AddResourceConsumption(consume);
        city.AddResource(stockpile);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount));

        for(var i=0; i<numberOfValidTurns; i++)
        {
            Assert.DoesNotThrow(() => { city.TurnUpdate(1); });
        }

        var missingResourceId = Assert.Throws<InsufficientResourceException>(() =>
        {
            city.TurnUpdate(1);
        });

        Assert.That(int.Parse(missingResourceId.Message), Is.EqualTo(DResource.NameToId(resourceName)));
    }

    [Test]
    public void MovePopulation()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building1 = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        var building2 = new DBuilding(city, BUILDING_NAME_2, MockBuildingController());
        var person = new DPerson(building1);

        Assert.That(building1.Population.Contains(person), Is.True);
        Assert.That(building2.Population.Contains(person), Is.False);
        Assert.That(person.Building, Is.EqualTo(building1));

        city.MovePerson(person, building2);

        Assert.That(building1.Population.Contains(person), Is.False);
        Assert.That(building2.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building2));
                
        city.MovePerson(person, building1);

        Assert.That(building1.Population.Contains(person), Is.True);
        Assert.That(building2.Population.Contains(person), Is.False);
        Assert.That(person.Building, Is.EqualTo(building1));

        var city2 = new DCity(CITY_NAME_2, MockCityController());
        var building3 = new DBuilding(city2, BUILDING_NAME_3, MockBuildingController());

        Assert.That(city2, Is.Not.EqualTo(city));
        Assert.Throws<BuildingNotInCityException>(() =>
        {
            city.MovePerson(person, building3);
        });
    }

    private static BuildingController MockBuildingController()
    {
        return new GameObject().AddComponent<BuildingController>().GetComponent<BuildingController>();
    }

    private static CityController MockCityController()
    {
        return new GameObject().AddComponent<CityController>().GetComponent<CityController>();
    }
}