using NUnit.Framework;
using NSubstitute;

public class GameManagerTests  {

    [Test]
    public void GameManagerTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        DGame gm = new DGame();
        Assert.That(gm.Cities, Is.Not.Null);
        Assert.That(gm.TurnDuration, Is.EqualTo(7));
        Assert.That(gm.TurnNumber, Is.EqualTo(0));
        Assert.That(gm.DaysTranspired, Is.EqualTo(0));


    }
    [Test]
    public void CityTurnUpdateTest()
    {
        var gm = new DGame();
        


        for (var i = 0; i < 10; i++)
        {
            Assert.That(gm.TurnNumber, Is.EqualTo(i));
            Assert.That(gm.DaysTranspired, Is.EqualTo(i*gm.TurnDuration));
            gm.EndTurnUpdate();
        }

    }

    [Test]
    public void AddCityTest()
    {
        var city = GetCityMock();
        var gm = new DGame();

        Assert.That(gm.Cities.Count, Is.EqualTo(0));

        gm.Cities.Add(city);
        Assert.That(gm.Cities.Count, Is.EqualTo(1));
    }

    [Test]
    public void MovePersonTest()
    {
        var city = GetCityMock();
        var gm = new DGame();
        gm.Cities.Add(city);
        var building1 = new DBuilding(city);
        var building2 = new DBuilding(city);
        var person = new DPerson(building1);
        building1.AddPerson(person);

        gm.MovePerson(person, building2);

        Assert.That(building1.Population.Contains(person), Is.False);
        Assert.That(building2.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building2));

        building2.RemovePerson(person);
        person = null;
        person = new DPerson();
        gm.MovePerson(person, building1);

        Assert.That(building1.Population.Contains(person), Is.True);
        Assert.That(person.Building, Is.EqualTo(building1));
    }


    private DCity GetCityMock()
    {
        return Substitute.For<DCity>();
    }
}
