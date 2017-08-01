using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCompressedCity : DBuilding, TurnUpdatable {

    private int population;
    private float percentInfected;
    private float development;
    private float[] prosperityMeasures = new float[(int)Constants._prosperityMeasures.NUMELEMENTS];
    private Dictionary<int, DResource> finalResources = new Dictionary<int, DResource>();
    private Dictionary<DResource, int> resourceRates = new Dictionary<DResource, int>();

    public DCompressedCity(DCity baseCity): base ( baseCity.Name)

    {
        population = baseCity.People.Count;
        percentInfected = baseCity.PercentPopulationInfected();
        development = baseCity.DevelopedValue();
        prosperityMeasures = CalculateProsperityMeasures(baseCity);
        finalResources = baseCity.Resources;
        resourceRates = baseCity.ResourceRates;
        baseCity.Resources = finalResources;    // keep if resource rates change city's resource levels, resets

    }
    public void assignCity(DCity city)
    {
      base.City = city;
      base.City.CityController.gameController.CreateBuildingController(this, new Vector3(1,1,0));

      // this.City.AddBuilding(this);
    }
    public override void TurnUpdate(int numDaysPassed)
    {
      Debug.Log("Last City stuff is in here.");
      foreach(var entry in  City.Resources)
        foreach (var rate in resourceRates)
        {
          if(entry.Value == rate.Key) City.AddResource(rate.Key, rate.Value);
        }
    }
    public float[] CalculateProsperityMeasures(DCity city)
    {
        float[] results = new float[(int)Constants._prosperityMeasures.NUMELEMENTS];

        results[(int)Constants._prosperityMeasures.HEALTH] = CalculateHealthProsperity(city);
        results[(int)Constants._prosperityMeasures.MORALE] = CalculateMoraleProsperity(city);
        results[(int)Constants._prosperityMeasures.ORDER] = CalculateOrderProsperity(city);
        results[(int)Constants._prosperityMeasures.EDUCATION] = CalculateEducationProsperity(city);

        return results;
    }

    // TODO - more complex calculation
    public float CalculateHealthProsperity(DCity city)
    {
        return 1f - city.PercentPopulationInfected();
    }

    // TODO - calculation
    public float CalculateMoraleProsperity(DCity city)
    {
        return 0f;
    }

    // TODO - calculation once order tasks implemented
    public float CalculateOrderProsperity(DCity city)
    {
        return 0f;
    }

    // TODO - calculation once education tasks implemented
    public float CalculateEducationProsperity(DCity city)
    {
        return 0f;
    }

    // stub for integration with city rate of resource change function(s)
    // public Dictionary<int, DResource>[] CalculateResourceRates(DCity city)
    // {
    //     Dictionary<int, DResource>[] results = new Dictionary<int, DResource>[(int)DSeasons._season.NUMELEMENTS];
    //
    //     return results;
    // }

    #region Properties
    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    public float PercentInfected
    {
        get { return percentInfected; }
        set { percentInfected = value; }
    }

    public float Development
    {
        get { return development; }
        set { development = value; }
    }

    public float[] ProsperityMeasures
    {
        get { return prosperityMeasures; }
        set { prosperityMeasures = value;  }
    }

    public Dictionary<int, DResource> FinalResources
    {
        get { return finalResources; }
        set { finalResources = value; }
    }

    public Dictionary<DResource, int> ResourceRates
    {
        get { return resourceRates; }
        set { resourceRates = value; }
    }
    #endregion
}
