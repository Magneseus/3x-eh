using NUnit.Framework;
using NSubstitute;

public class BuildingTests
{

    [Test]
    public void BuildingTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void InitializesDefaultValues()
    {
        var city = new City();
        var building = new Building(city);

        Assert.That(building.City.Name, Is.EqualTo(city.Name));
        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(0));
        Assert.That(building.ResourceOutput.Count, Is.EqualTo(0));
        Assert.That(building.Population.Count, Is.EqualTo(0));
        Assert.That(building.Name, Is.EqualTo(""));
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
        var city = new City();
        var building = new Building(city);

        var consumptionName = "Test";
        var consumptionAmount = 5;
        var consumption = Resource.Create(consumptionName, consumptionAmount);

        building.AddResourceConsumption(consumption);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(1));
        Assert.That(building.ResourceConsumption[consumption.Id].Amount, Is.EqualTo(consumption.Amount));
    }

    [Test]
    public void AddMultipleConsumptionSameResource()
    {
        var city = new City();
        var building = new Building(city);

        var consumptionName = "Test";
        var consumptionAmount = 5;
        var consumption = Resource.Create(consumptionName, consumptionAmount);

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
        var city = new City();
        var building = new Building(city);

        var consumptionOneName = "Test";
        var consumptionOneAmount = 5;
        var consumptionOne = Resource.Create(consumptionOneName, consumptionOneAmount);

        var consumptionTwoName = "Other Test";
        var consumptionTwoAmount = 3;
        var consumptionTwo = Resource.Create(consumptionTwoName, consumptionTwoAmount);

        
        building.AddResourceConsumption(consumptionOne);
        building.AddResourceConsumption(consumptionTwo);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(2));
        Assert.That(building.ResourceConsumption[consumptionOne.Id].Amount, Is.EqualTo(consumptionOne.Amount));
        Assert.That(building.ResourceConsumption[consumptionTwo.Id].Amount, Is.EqualTo(consumptionTwo.Amount));
    }    

    [Test]
    public void AddMultipleConsumptionDifferentResource()
    {
        var city = new City();
        var building = new Building(city);

        var consumptionOneName = "Test";
        var consumptionOneAmount = 5;
        var consumptionOne = Resource.Create(consumptionOneName, consumptionOneAmount);

        var consumptionTwoName = "Other Test";
        var consumptionTwoAmount = 3;
        var consumptionTwo = Resource.Create(consumptionTwoName, consumptionTwoAmount);

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
        var stockpile = Resource.Create(resourceName, stockpileAmount);

        var consumeAmount = 3;
        var consume = Resource.Create(resourceName, consumeAmount);

        var city = new City();
        var building = new Building(city);

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

        var stockpile = Resource.Create(resourceName, stockpileAmount);        
        var consume = Resource.Create(resourceName, consumeAmount);

        var city = new City();
        city.AddResource(stockpile);

        var building = new Building(city);
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
        var city = new City();
        var building = new Building(city);

        var outputName = "Test";
        var outputAmount = 5;
        var output = Resource.Create(outputName, outputAmount);

        building.AddResourceOutput(output);

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(1));
        Assert.That(building.ResourceOutput[output.Id].Amount, Is.EqualTo(output.Amount));
    }

    [Test]
    public void AddMultipleOutputSameResource()
    {
        var city = new City();
        var building = new Building(city);

        var outputName = "Test";
        var outputAmount = 5;
        var output = Resource.Create(outputName, outputAmount);

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
        var city = new City();
        var building = new Building(city);

        var outputOneName = "Test";
        var outputOneAmount = 5;
        var outputOne = Resource.Create(outputOneName, outputOneAmount);

        var outputTwoName = "Other Test";
        var outputTwoAmount = 3;
        var outputTwo = Resource.Create(outputTwoName, outputTwoAmount);


        building.AddResourceOutput(outputOne);
        building.AddResourceOutput(outputTwo);

        Assert.That(building.ResourceOutput.Count, Is.EqualTo(2));
        Assert.That(building.ResourceOutput[outputOne.Id].Amount, Is.EqualTo(outputOne.Amount));
        Assert.That(building.ResourceOutput[outputTwo.Id].Amount, Is.EqualTo(outputTwo.Amount));
    }

    [Test]
    public void AddMultipleOutputDifferentResource()
    {
        var city = new City();
        var building = new Building(city);

        var outputOneName = "Test";
        var outputOneAmount = 5;
        var outputOne = Resource.Create(outputOneName, outputOneAmount);

        var outputTwoName = "Other Test";
        var outputTwoAmount = 3;
        var outputTwo = Resource.Create(outputTwoName, outputTwoAmount);

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
        var stockpile = Resource.Create(resourceName, stockpileAmount);

        var outputAmount = 2;
        var output = Resource.Create(resourceName, outputAmount);

        var city = new City();
        var building = new Building(city);

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

        var stockpile = Resource.Create(resourceName, stockpileAmount);
        var output = Resource.Create(resourceName, outputAmount);

        var city = new City();
        city.AddResource(stockpile);

        var building = new Building(city);
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
        var city = new City();
        var building = new Building(city);
        var person = new Person();
        var personCount = building.Population.Count;

        Assert.That(person.Building, Is.Null);
        Assert.That(building.Population.Count, Is.EqualTo(personCount));

        building.AddPerson(person);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount + 1));
    }

    [Test]
    public void OverAddPopulation()
    {
        var city = new City();
        var building = new Building(city);
        var person = new Person(building);
        var personCount = building.Population.Count;

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount));

        building.AddPerson(person);

        Assert.That(person.Building, Is.EqualTo(building));
        Assert.That(building.Population.Count, Is.EqualTo(personCount));
    }

    [Test]
    public void RemovePopulation()
    {
        var city = new City();
        var building = new Building(city);
        var person = new Person();

        building.AddPerson(person);
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
        var city = new City();
        var building = new Building(city);

        var resourceName = "Test";
        var outputAmount = 3;
        var output = Resource.Create(resourceName, outputAmount);

        building.AddResourceOutput(output);

        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(0));
        building.TurnUpdate(1);
        Assert.That(city.GetResource(resourceName).Amount, Is.EqualTo(outputAmount));


        building.Population.Add(new Person(building));
        building.TurnUpdate(1);
        Assert.That(city.GetResource(resourceName).Amount, Is.GreaterThan(outputAmount * 2));
    }
    #endregion
}