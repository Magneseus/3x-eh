using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class testing : MonoBehaviour {

    private string CITY_NAME = "Test City";
    private string LINKED_CITY_NAME = "Linked City";
    private string BUILDING_NAME = "Test Building";
    private string BUILDING_NAME_2 = "Other Test Building";
    private DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };
    private DateTime currentDate = new DateTime(2017, 1, 1);
    private DateTime[] defaultDeadOfWinterDates = { new DateTime(2017, 1, 7), new DateTime(2017, 1, 21) };
    private DateTime[] lateWinterSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 1, 5) };
    private DateTime[] zeroYearSeasonStartDates = { new DateTime(1000, 4, 1), new DateTime(1000, 6, 1), new DateTime(1000, 8, 1), new DateTime(1000, 12, 1) };
    private DateTime[] zeroYearLateWinterSeasonStartDates = { new DateTime(1000, 4, 1), new DateTime(1000, 6, 1), new DateTime(1000, 8, 1), new DateTime(1000, 1, 5) };


    private void Awake()
    {

        var city = new DCity(CITY_NAME, new CityController(), defaultSeasonStartDates, DateTime.Now);
        var numberOfDaysPassed = 7;

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);
        int numFood = 50;
        city.AddResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), numFood);
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(numFood));
        city.Season = DSeasons._season.WINTER;

        int baseConsume = 10;

        city.ConsumeResource(city.GetResource(Constants.FOOD_RESOURCE_NAME), baseConsume);
        int expected = numFood - (int)(baseConsume * city.SeasonResourceConsumedMod(city.GetResource(Constants.FOOD_RESOURCE_NAME)));
        Assert.That(city.GetResource(Constants.FOOD_RESOURCE_NAME).Amount, Is.EqualTo(expected));
    }
}
