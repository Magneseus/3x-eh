using NUnit.Framework;
using NSubstitute;
using UnityEngine;

public class BuildingTests
{
    private string CITY_NAME = "Test City";
    private string BUILDING_NAME = "Test Building";

    [Test]
    public void BuildingTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        Assert.That(building.City.Name, Is.EqualTo(city.Name));
        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(0));
        Assert.That(building.ResourceOutput.Count, Is.EqualTo(0));
        Assert.That(building.Population.Count, Is.EqualTo(0));
        Assert.That(building.Name, Is.EqualTo(BUILDING_NAME));
    }    

    [Test]
    public void Name()
    {
        var newName = "Test123";
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController())
        {
           Name = newName
        };

        Assert.That(building.Name, Is.EqualTo(newName));
    }

    #region Consumption Tests
    /*****************************
     * 
     *      Consumption Tests
     *         
     *****************************/
    [Test]
    public void AddSingleConsumption()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var consumptionName = "Test";
        var consumptionAmount = 5;
        var consumption = DResource.Create(consumptionName, consumptionAmount);

        building.AddResourceConsumption(consumption);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(1));
        Assert.That(building.ResourceConsumption[consumption.Id].Amount, Is.EqualTo(consumption.Amount));
    }

    [Test]
    public void AddMultipleConsumptionSameResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var consumptionName = "Test";
        var consumptionAmount = 5;
        var consumption = DResource.Create(consumptionName, consumptionAmount);

        var numberOfAdds = 5;
        for(var i=0; i<numberOfAdds; i++)
        {
            building.AddResourceConsumption(consumption);
        }        

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(1));
        Assert.That(building.ResourceConsumption[consumption.Id].Amount, Is.EqualTo(consumption.Amount * numberOfAdds));
    }


    [Test]
    public void AddSingleConsumptionDifferentResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var consumptionOneName = "Test";
        var consumptionOneAmount = 5;
        var consumptionOne = DResource.Create(consumptionOneName, consumptionOneAmount);

        var consumptionTwoName = "Other Test";
        var consumptionTwoAmount = 3;
        var consumptionTwo = DResource.Create(consumptionTwoName, consumptionTwoAmount);

        
        building.AddResourceConsumption(consumptionOne);
        building.AddResourceConsumption(consumptionTwo);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(2));
        Assert.That(building.ResourceConsumption[consumptionOne.Id].Amount, Is.EqualTo(consumptionOne.Amount));
        Assert.That(building.ResourceConsumption[consumptionTwo.Id].Amount, Is.EqualTo(consumptionTwo.Amount));
    }    

    [Test]
    public void AddMultipleConsumptionDifferentResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var consumptionOneName = "Test";
        var consumptionOneAmount = 5;
        var consumptionOne = DResource.Create(consumptionOneName, consumptionOneAmount);

        var consumptionTwoName = "Other Test";
        var consumptionTwoAmount = 3;
        var consumptionTwo = DResource.Create(consumptionTwoName, consumptionTwoAmount);

        var numberOfAdds = 5;
        for (var i = 0; i < numberOfAdds; i++)
        {
            building.AddResourceConsumption(consumptionOne);
            building.AddResourceConsumption(consumptionTwo);
        }

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(2));
        Assert.That(building.ResourceConsumption[consumptionOne.Id].Amount, Is.EqualTo(consumptionOne.Amount * numberOfAdds));
        Assert.That(building.ResourceConsumption[consumptionTwo.Id].Amount, Is.EqualTo(consumptionTwo.Amount * numberOfAdds));
    }

    [Test]
    public void ConsumeResourcesSingleTurn()
    {        
        var resourceName = "Test";
        var stockpileAmount = 10;
        var stockpile = DResource.Create(resourceName, stockpileAmount);

        var consumeAmount = 3;
        var consume = DResource.Create(resourceName, consumeAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        city.AddResource(stockpile);
        building.AddResourceConsumption(consume);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount));

        building.TurnUpdate(1);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount - consumeAmount));
    }

    [Test]
    public void ConsumeResourcesMultipleTurns()
    {
        var numberOfTurns = 5;
        var resourceName = "Test";

        var consumeAmount = 3;
        var stockpileAmount = consumeAmount * numberOfTurns;

        var stockpile = DResource.Create(resourceName, stockpileAmount);        
        var consume = DResource.Create(resourceName, consumeAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        city.AddResource(stockpile);
        building.AddResourceConsumption(consume);

        
        for (var i=0; i<numberOfTurns; i++)
        {
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount - (consumeAmount * i)));
            building.TurnUpdate(1);
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount - (consumeAmount * (i+1))));
        }
    }


    #endregion


    #region Output Tests
    /*****************************
     * 
     *         Output Tests
     *         
     *****************************/
    [Test]
    public void AddSingleOutput()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var outputName = "Test";
        var outputAmount = 5;
        var output = DResource.Create(outputName, outputAmount);

        building.AddResourceOutput(output);

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(1));
        Assert.That(building.ResourceOutput[output.Id].Amount, Is.EqualTo(output.Amount));
    }

    [Test]
    public void AddMultipleOutputSameResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var outputName = "Test";
        var outputAmount = 5;
        var output = DResource.Create(outputName, outputAmount);

        var numberOfAdds = 5;
        for (var i = 0; i < numberOfAdds; i++)
        {
            building.AddResourceOutput(output);
        }

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(1));
        Assert.That(building.ResourceOutput[output.Id].Amount, Is.EqualTo(output.Amount * numberOfAdds));
    }


    [Test]
    public void AddSingleOutputDifferentResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var outputOneName = "Test";
        var outputOneAmount = 5;
        var outputOne = DResource.Create(outputOneName, outputOneAmount);

        var outputTwoName = "Other Test";
        var outputTwoAmount = 3;
        var outputTwo = DResource.Create(outputTwoName, outputTwoAmount);


        building.AddResourceOutput(outputOne);
        building.AddResourceOutput(outputTwo);

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(2));
        Assert.That(building.ResourceOutput[outputOne.Id].Amount, Is.EqualTo(outputOne.Amount));
        Assert.That(building.ResourceOutput[outputTwo.Id].Amount, Is.EqualTo(outputTwo.Amount));
    }

    [Test]
    public void AddMultipleOutputDifferentResource()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var outputOneName = "Test";
        var outputOneAmount = 5;
        var outputOne = DResource.Create(outputOneName, outputOneAmount);

        var outputTwoName = "Other Test";
        var outputTwoAmount = 3;
        var outputTwo = DResource.Create(outputTwoName, outputTwoAmount);

        var numberOfAdds = 5;        
        for (var i = 0; i < numberOfAdds; i++)
        {
            building.AddResourceOutput(outputOne);
            building.AddResourceOutput(outputTwo);
        }

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(2));
        Assert.That(building.ResourceOutput[outputOne.Id].Amount, Is.EqualTo(outputOne.Amount * numberOfAdds));
        Assert.That(building.ResourceOutput[outputTwo.Id].Amount, Is.EqualTo(outputTwo.Amount * numberOfAdds));
    }

    [Test]
    public void OutputResourcesSingleTurn()
    {
        var resourceName = "Test";
        var stockpileAmount = 5;
        var stockpile = DResource.Create(resourceName, stockpileAmount);

        var outputAmount = 2;
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        city.AddResource(stockpile);
        building.AddResourceOutput(output);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount));

        building.TurnUpdate(1);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount + outputAmount));
    }

    [Test]
    public void OutputResourcesMultipleTurns()
    {
        var numberOfTurns = 5;
        var resourceName = "Test";

        var outputAmount = 3;
        var stockpileAmount = 2;

        var stockpile = DResource.Create(resourceName, stockpileAmount);
        var output = DResource.Create(resourceName, outputAmount);

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        city.AddResource(stockpile);
        building.AddResourceOutput(output);


        for (var i = 0; i < numberOfTurns; i++)
        {
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount + (outputAmount * i)));
            building.TurnUpdate(1);
            Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(stockpileAmount + (outputAmount * (i + 1))));
        }
    }
    #endregion


    #region Population Modifiers
    [Test]
    public void AddPopulation()
    {
        var personCount = 0;

        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());        
        Assert.That(building.Population.Count, Is.EqualTo(personCount));

        var person = new DPerson(building);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount + 1));

        var person2 = new DPerson(building);
        Assert.That(person2.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount + 2));
    }

    [Test]
    public void OverAddPopulation()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        var person = new DPerson(building);
        var personCount = building.Population.Count;

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount));

        building.AddPersonToBuilding(person);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount));
    }

    [Test]
    public void RemovePopulation()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());
        var person = new DPerson(building);

        building.AddPersonToBuilding(person);
        var personCount = building.Population.Count;

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount));

        building.RemovePerson(person);

        Assert.That(person.Building, Is.Null);
        Assert.That(building.Population.Count, Is.EqualTo(personCount - 1));

        Assert.Throws<PersonNotFoundException>(() =>
        {
            building.RemovePerson(person);
        });
    }

    [Test]
    public void PopulationIncreasesResourceGeneration()
    {
        var city = new DCity(CITY_NAME, MockCityController());
        var building = new DBuilding(city, BUILDING_NAME, MockBuildingController());

        var resourceName = "Test";
        var outputAmount = 3;
        var output = DResource.Create(resourceName, outputAmount);

        building.AddResourceOutput(output);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(0));
        building.TurnUpdate(1);
        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount));


        building.Population.Add(new DPerson(building));
        building.TurnUpdate(1);
        Assert.That(city.GetResource(resourceName).Amount, Is.GreaterThan(outputAmount * 2));
    }
    #endregion

    private static BuildingController MockBuildingController()
    {
        return new GameObject().AddComponent<BuildingController>().GetComponent<BuildingController>();
    }

    private static CityController MockCityController()
    {
        return new GameObject().AddComponent<CityController>().GetComponent<CityController>();
    }
}