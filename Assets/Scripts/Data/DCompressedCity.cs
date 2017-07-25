using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCompressedCity {

    private int population;
    private float development;
    private Constants._prosperityMeasures[] prosperityMeasures = 
        new Constants._prosperityMeasures[(int)Constants._prosperityMeasures.NUMELEMENTS];
    private Dictionary<int, DResource> finalResources = new Dictionary<int, DResource>();
    private Dictionary<int, DResource> resourceRates = new Dictionary<int, DResource>();

    #region Properties
    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    public float Development
    {
        get { return development; }
        set { development = value; }
    }

    public Constants._prosperityMeasures[] ProsperityMeasures
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
