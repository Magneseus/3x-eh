using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCompressedCity {

    private int population;
    private float percentInfected;
    private float development;
    private float[] prosperityMeasures = 
        new float[(int)Constants._prosperityMeasures.NUMELEMENTS];
    private Dictionary<int, DResource> finalResources = new Dictionary<int, DResource>();
    private Dictionary<int, DResource> resourceRates = new Dictionary<int, DResource>();

    public DCompressedCity(DCity baseCity)
    {
        population = baseCity.People.Count;
        percentInfected = baseCity.PercentPopulationInfected();
        //TODO - percent development of cities based on amount reclaimed
        prosperityMeasures = CalculateProsperityMeasures(baseCity);
        finalResources = baseCity.Resources;
        //TODO - calculate resource rates
    }

    public float[] CalculateProsperityMeasures(DCity city)
    {
        float[] results =
            new float[(int)Constants._prosperityMeasures.NUMELEMENTS];

        //results[(int)Constants._prosperityMeasures.HEALTH]

        return results;
    }





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

    public Dictionary<int, DResource> ResourceRates
    {
        get { return resourceRates; }
        set { resourceRates = value; }
    }
    #endregion
}
