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
        Assert.That(gm.ListOfCities, Is.Not.Null);
        Assert.That(gm.DurationOfTurn, Is.EqualTo(7));
        Assert.That(gm.CurrentTurnNumber, Is.EqualTo(0));
        Assert.That(gm.DaysTranspired, Is.EqualTo(7));


    }
    [Test]
    public void CityTurnUpdateTest()
    {
        var gm = new GameManager();
        


        for (var i = 0; i < 10; i++)
        {
            Assert.That(gm.CurrentTurnNumber, Is.EqualTo(i));
            Assert.That(gm.DaysTranspired, Is.EqualTo(i*gm.DurationOfTurn));
            gm.EndTurnUpdate();
        }

    }

    [Test]
    public void AddCityTest()
    {
        var city = GetCityMock();
        var gm = new GameManager();

        Assert.That(gm.ListOfCities.Count, Is.EqualTo(0));

        gm.ListOfCities.Add(city);
        Assert.That(gm.ListOfCities.Count, Is.EqualTo(1));
    }


    private City GetCityMock()
    {
        return Substitute.For<City>();
    }
}
