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
        var city = new City();
        Assert.That(city.Buildings, Is.Not.Null);
        Assert.That(city.Resources, Is.Not.Null);
        Assert.That(city.Population, Is.Not.Null);
        Assert.That(city.Name, Is.EqualTo("NoCityName"));
        Assert.That(city.CivilianCount, Is.EqualTo(0));
        Assert.That(city.Age, Is.EqualTo(0));
    }

    [Test]
    public void Name()
    {
        var newName = "Test123";
        var city = new City()
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuilding()
    {        
        var city = new City();
        var building = new Building(city);

        Assert.That(city.Buildings.Count, Is.EqualTo(0));

        city.Buildings.Add(building);
        Assert.That(city.Buildings.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddResource()
    {
        var resource = Resource.Create("Test", 0);
        var city = new City();

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.AddResource(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddPopulation()
    {
        var person = new Person();
        var city = new City();

        Assert.That(city.Population.Count, Is.EqualTo(0));

        city.Population.Add(person);
        Assert.That(city.Population.Count, Is.EqualTo(1));
    }

    [Test]
    public void CivilianCount()
    {
        var person = new Person();
        var city = new City();
        Assert.That(city.CivilianCount, Is.EqualTo(0));
        city.Population.Add(person);
        Assert.That(city.CivilianCount, Is.EqualTo(1));

    }


    [Test]
    public void TurnUpdateDaysPassed()
    {
        var city = new City();
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
        var output = Resource.Create(resourceName, outputAmount);

        var city = new City();
        var building = new Building(city);
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
        var output = Resource.Create(resourceName, outputAmount);

        var city = new City();
        var building = new Building(city);
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
        var consume = Resource.Create(resourceName, consumeAmount);

        var stockpileAmount = 2;
        var stockpile = Resource.Create(resourceName, stockpileAmount);
        
        
        var city = new City();
        var building = new Building(city);
        
        building.AddResourceConsumption(consume);
        city.Buildings.Add(building);
        city.AddResource(stockpile);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount));

        var missingResourceId = Assert.Throws<InsufficientResourceException>(() =>
        {
            city.TurnUpdate(1);
        });

        Assert.That(int.Parse(missingResourceId.Message), Is.EqualTo(Resource.NameToId(resourceName)));        
    }

    [Test]
    public void TurnUpdateInsufficientResourcesAfterMultipleTurns()
    {
        var numberOfValidTurns = 4;
        var resourceName = "Test";

        var consumeAmount = 3;
        var consume = Resource.Create(resourceName, consumeAmount);

        var stockpileAmount = numberOfValidTurns * consumeAmount + 1;
        var stockpile = Resource.Create(resourceName, stockpileAmount);


        var city = new City();
        var building = new Building(city);

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

        Assert.That(int.Parse(missingResourceId.Message), Is.EqualTo(Resource.NameToId(resourceName)));
    }
}