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
        GameManager gm = new GameManager();
        Assert.That(gm.Cities, Is.Not.Null);
        Assert.That(gm.TurnDuration, Is.EqualTo(7));
        Assert.That(gm.TurnNumber, Is.EqualTo(0));
        Assert.That(gm.DaysTranspired, Is.EqualTo(0));


    }
    [Test]
    public void CityTurnUpdateTest()
    {
        var gm = new GameManager();
        


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
        var gm = new GameManager();

        Assert.That(gm.Cities.Count, Is.EqualTo(0));

        gm.Cities.Add(city);
        Assert.That(gm.Cities.Count, Is.EqualTo(1));
    }


    private City GetCityMock()
    {
        return Substitute.For<City>();
    }
}
