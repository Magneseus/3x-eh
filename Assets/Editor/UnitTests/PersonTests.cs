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
        var city = new DCity();
        var building = new DBuilding(city);
        var person = new DPerson(building);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Contains(person), Is.True);

        // Check 0 arg constructor as well
        person = null;
        person = new DPerson();

        Assert.That(person.Building, Is.Null);
    }
}
