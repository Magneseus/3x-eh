using NUnit.Framework;
using NSubstitute;

public class PersonTests
{
    [Test]
    public void PersonTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new City();
        var building = new Building(city);
        var person = new Person(building);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Contains(person), Is.True);

        // Check 0 arg constructor as well
        person = null;
        person = new Person();

        Assert.That(person.Building, Is.Null);
    }
}
