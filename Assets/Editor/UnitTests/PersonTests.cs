using NUnit.Framework;
using NSubstitute;
using UnityEngine;

public class PersonTests
{
    private string CITY_NAME = "Test City";
    private string BUILDING_NAME = "Test Building";
    
    [Test]
    public void PersonTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        var person = new DPerson(building);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Contains(person), Is.True);        
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
