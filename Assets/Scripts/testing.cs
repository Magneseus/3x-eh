using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NUnit.Framework;

public class testing : MonoBehaviour {

    class Mock
    {
        private static List<GameObject> mockObjects = new List<GameObject>();

        public static void TearDown()
        {
            foreach (var entry in mockObjects)
                UnityEngine.Object.DestroyImmediate(entry);
        }

        public static DTask CleanTask(DBuilding building, DResource resource)
        {
            var task = new DTask(building, resource);

            task.ForceClean();
            task.ForceFixed();

            return task;
        }

        public static T Component<T>() where T : Component
        {
            var mockObj = new GameObject();
            mockObjects.Add(mockObj);
            return mockObj.AddComponent<T>().GetComponent<T>();
        }

    }

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

    private static string TOWN_HALL = "Town Hall";
    private static string RESOURCE_NAME = "Test Resource";
    private static int RESOURCE_START_AMOUNT = 3;
    

    private void Awake()
    {

        var resource = DResource.Create(RESOURCE_NAME, RESOURCE_START_AMOUNT);

        var city = new DCity(CITY_NAME, Mock.Component<CityController>(), defaultSeasonStartDates, DateTime.Now);
        var townHall = new DBuilding(city, TOWN_HALL, Mock.Component<BuildingController>());
        var building = new DBuilding(city, BUILDING_NAME, Mock.Component<BuildingController>());
        var task = Mock.CleanTask(building, resource);
        var person = new DPerson(city, Mock.Component<MeepleController>());
        person.SetTask(task);

        Debug.Log(task.Output.Amount);

        // temp - creating default food resource needed for city.turnupdate to work
        DResource.Create(Constants.FOOD_RESOURCE_NAME);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(0));

        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));

        task.DisableTask();
        city.TurnUpdate(1);

        Assert.That(city.GetResource(RESOURCE_NAME).Amount, Is.EqualTo(RESOURCE_START_AMOUNT));
    }
}
