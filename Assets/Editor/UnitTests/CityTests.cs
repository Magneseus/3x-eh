using System;
using NUnit.Framework;
using NSubstitute;

public class CityTests
{

    [Test]
    public void CityTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity();
        Assert.That(city.Buildings, Is.Not.Null);
        Assert.That(city.Resources, Is.Not.Null);
        Assert.That(city.Population, Is.Not.Null);
        Assert.That(city.Name, Is.EqualTo("NoCityName"));
        Assert.That(city.CivilianCount, Is.EqualTo(0));
        Assert.That(city.Age, Is.EqualTo(0));
        Assert.That(city.EmptyBuilding, Is.Not.Null);
    }

    [Test]
    public void Name()
    {
        var newName = "Test123";
        var city = new DCity()
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuilding()
    {        
        var city = new DCity();
        var startingBuildingCount = city.Buildings.Count;
        var building = new DBuilding(city);

        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount));

        city.Buildings.Add(building);
        Assert.That(city.Buildings.Count, Is.EqualTo(startingBuildingCount + 1));
    }

    [Test]
    public void AddResource()
    {
        var resource = DResource.Create("Test", 0);
        var city = new DCity();

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.AddResource(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddPopulation()
    {
        var person = new DPerson();
        var city = new DCity();

        Assert.That(city.Population.Count, Is.EqualTo(0));

        city.Population.Add(person);
        Assert.That(city.Population.Count, Is.EqualTo(1));
    }

    [Test]
    public void CivilianCount()
    {
        var person = new DPerson();
        var city = new DCity();
        Assert.That(city.CivilianCount, Is.EqualTo(0));
        city.Population.Add(person);
        Assert.That(city.CivilianCount, Is.EqualTo(1));

    }


    [Test]
    public void TurnUpdateDaysPassed()
    {
        var city = new DCity();
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

        var city = new DCity();
        var building = new DBuilding(city);
        building.AddResourceOutput(output);
        city.Buildings.Add(building);

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

        var city = new DCity();
        var building = new DBuilding(city);
        building.AddResourceOutput(output);
        city.Buildings.Add(building);
        
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
        
        
        var city = new DCity();
        var building = new DBuilding(city);
        
        building.AddResourceConsumption(consume);
        city.Buildings.Add(building);
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


        var city = new DCity();
        var building = new DBuilding(city);

        building.AddResourceConsumption(consume);
        city.Buildings.Add(building);
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
        var city = new DCity();
        var building1 = new DBuilding(city);
        var building2 = new DBuilding(city);
        var person = new DPerson(building1);
        building1.AddPerson(person);

        city.MovePerson(person, building2);

        Assert.That(building1.Population.Contains(person), Is.False);
        Assert.That(building2.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building2));

        building2.RemovePerson(person);
        person = null;
        person = new DPerson();
        city.MovePerson(person, building1);

        Assert.That(building1.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building1));

        var city2 = new DCity();
        var building3 = new DBuilding(city2);

        Assert.Throws<BuildingNotInCityException>(() =>
        {
            city.MovePerson(person, building3);
        });
    }
}