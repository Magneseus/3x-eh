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
    public void NameTest()
    {
        var newName = "Test123";
        var city = new City()
        {
            Name = newName
        };
        Assert.That(city.Name, Is.EqualTo(newName));
    }

    [Test]
    public void AddBuildingTest()
    {        
        var city = new City();
        var building = new Building(city);

        Assert.That(city.Buildings.Count, Is.EqualTo(0));

        city.Buildings.Add(building);
        Assert.That(city.Buildings.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddResourceTest()
    {
        var resource = Resource.Create("Test", 0);
        var city = new City();

        Assert.That(city.Resources.Count, Is.EqualTo(0));

        city.Resources.Add(resource);
        Assert.That(city.Resources.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddPopulationTest()
    {
        var person = new Person();
        var city = new City();

        Assert.That(city.Population.Count, Is.EqualTo(0));

        city.Population.Add(person);
        Assert.That(city.Population.Count, Is.EqualTo(1));
    }

    [Test]
    public void CivilianCountTest()
    {
        var person = new Person();
        var city = new City();
        Assert.That(city.CivilianCount, Is.EqualTo(0));
        city.Population.Add(person);
        Assert.That(city.CivilianCount, Is.EqualTo(1));

    }


    [Test]
    public void CityTurnUpdateTest()
    {
        var city = new City();
        var numberOfDaysPassed = 7;


        for (var i = 0; i < 10; i++)
        {
            Assert.That(city.Age, Is.EqualTo(i * numberOfDaysPassed));
            city.TurnUpdate(numberOfDaysPassed);
        }

    }
}