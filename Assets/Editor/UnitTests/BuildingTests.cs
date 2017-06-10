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
        Assert.That(building.PopulationCapacity, Is.EqualTo(0));
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

        building.AddResourceConsumpTion(consumption);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(1));
        Assert.That(building.ResourceConsumption[consumption.Id], Is.EqualTo(consumption.Amount));
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
            building.AddResourceConsumpTion(consumption);
        }        

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(1));
        Assert.That(building.ResourceConsumption[consumption.Id], Is.EqualTo(consumption.Amount * numberOfAdds));
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

        
        building.AddResourceConsumpTion(consumptionOne);
        building.AddResourceConsumpTion(consumptionTwo);

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(2));
        Assert.That(building.ResourceConsumption[consumptionOne.Id], Is.EqualTo(consumptionOne.Amount));
        Assert.That(building.ResourceConsumption[consumptionTwo.Id], Is.EqualTo(consumptionTwo.Amount));
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
            building.AddResourceConsumpTion(consumptionOne);
            building.AddResourceConsumpTion(consumptionTwo);
        }

        Assert.That(building.ResourceConsumption.Count, Is.EqualTo(2));
        Assert.That(building.ResourceConsumption[consumptionOne.Id], Is.EqualTo(consumptionOne.Amount * numberOfAdds));
        Assert.That(building.ResourceConsumption[consumptionTwo.Id], Is.EqualTo(consumptionTwo.Amount * numberOfAdds));
    }

    //[Test]
    //public void ConsumeOnSingleTurnUpdate()
    //{
    //    var city = new City();
    //    var building = new Building(city);

    //    var consumptionOneName = "Test";
    //    var consumptionOneAmount = 5;
    //    var consumptionOne = Resource.Create(consumptionOneName, consumptionOneAmount);

    //    Assert.Fail();




    //}
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
        Assert.That(building.ResourceOutput[output.Id], Is.EqualTo(output.Amount));
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
        Assert.That(building.ResourceOutput[output.Id], Is.EqualTo(output.Amount * numberOfAdds));
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
        Assert.That(building.ResourceOutput[outputOne.Id], Is.EqualTo(outputOne.Amount));
        Assert.That(building.ResourceOutput[outputTwo.Id], Is.EqualTo(outputTwo.Amount));
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
        Assert.That(building.ResourceOutput[outputOne.Id], Is.EqualTo(outputOne.Amount * numberOfAdds));
        Assert.That(building.ResourceOutput[outputTwo.Id], Is.EqualTo(outputTwo.Amount * numberOfAdds));
    }
    #endregion    
}