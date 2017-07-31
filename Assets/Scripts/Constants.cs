using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {

    

    public static string OTTAWA_PREFAB_PATH = @"Prefabs/Cities/Ottawa";
    public static string CITY_PREFAB_PATH = @"Prefabs/Cities/City";
    public static string CITY_SPRITE_PATH = @"Sprites/";

    public static string BUILDING_PREFAB_PATH = @"Prefabs/Buildings/Building";
    public static string TASK_TRAY_PREFAB_PATH = @"Prefabs/Tasks/TaskTray";
    public static string TASK_TRAY_SINGLE_PREFAB_PATH = @"Prefabs/Tasks/TaskTraySingle";
    public static string MEEPLE_PREFAB_PATH = @"Prefabs/MeepleController";

    public static string CITY_JSON_PATH = @"Assets/Resources/Data/starting_cities";
    public const string SAVE_JSON_PATH = @"Assets/Resources/Data/saved_games";

    public static string FOOD_RESOURCE_NAME = "Food";
    public static DateTime[] DEFAULT_SEASON_DATES = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };
    public static DateTime DEFAULT_DATE = DEFAULT_SEASON_DATES[0].AddDays(1);
    public static int DEFAULT_RESOURCE_VALUE = 0;

    public const int NO_INPUT = 0;
    public const int EVENT_PRIORITY_DEFAULT = 0;
    public const int EVENT_PRIORITY_INTERESTING = 1;
    public const int EVENT_PRIORITY_REQ_RESOURCES = 2;
    public const int EVENT_PRIORITY_STORY = 3;

    #region Task Constants
    public static float TEMP_REPAIR_AMOUNT = 0.2f;
    public static float DEFAULT_ASSESS_AMOUNT = 0.1f;

    public static float TASK_MIN_STRUCTURAL_DMG = 0.0f;
    public static float TASK_MAX_STRUCTURAL_DMG = 1.0f;
    public static float BUILDING_MIN_FUNGAL_DMG = 0.0f;
    public static float BUILDING_MAX_FUNGAL_DMG = 1.0f;

    public static float BUILDING_MERSON_INFECTION_WEIGHT = 0.25f;
    public static float TASK_MIN_FUNGAL_DMG = 0.0f;
    public static float TASK_MAX_FUNGAL_DMG = 1.0f;
    #endregion

    #region Merson Infection
    public static int MERSON_INFECTION_MIN = 0;
    public static int MERSON_INFECTION_MAX = 2;
    public static float MERSON_INFECTION_PROBABILITY = 0.05f;
    public static int MERSON_SPRING_FALL_INFECTION_MODIFIER = 1;
    public static float MERSON_INFECTION_TASK_MODIFIER = 0.5f;
    public static int[] MERSON_SEASON_INFECTION_MAX = { 1, MERSON_INFECTION_MAX, 1, MERSON_INFECTION_MIN };
    #endregion

    #region City and Compressed City
    public enum _prosperityMeasures { HEALTH, MORALE, ORDER, EDUCATION, NUMELEMENTS };
    public static float CITY_DEVELOPMENT_PERCENT_FROM_EXPLORE = 0.2f;
    public static float CITY_DEVELOPMENT_PERCENT_FROM_ASSESS = 0.4f;
    public static float CITY_DEVELOPMENT_PERCENT_FROM_REPAIR = 0.4f;
    #endregion
}
