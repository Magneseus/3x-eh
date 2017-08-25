using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {


    #region Paths
    public static string OTTAWA_PREFAB_PATH = @"Prefabs/Cities/Ottawa";
    public static string CITY_PREFAB_PATH = @"Prefabs/Cities/City";
    public static string CITY_SPRITE_PATH = @"Sprites/";

    public static string BUILDING_PREFAB_PATH = @"Prefabs/Buildings/Building";
    public static string BUILDING_PREFAB_PATH_MED_00 = @"Prefabs/Buildings/Building_Med_00";
    public static string BUILDING_PREFAB_PATH_MED_01 = @"Prefabs/Buildings/Building_Med_01";
    public static string[] BUILDING_PREFAB_PATHS_MED = { BUILDING_PREFAB_PATH_MED_00, BUILDING_PREFAB_PATH_MED_01 };

    public static string BUILDING_PREFAB_PATH_SM_00 = @"Prefabs/Buildings/Building_Small_00";
    public static string[] BUILDING_PREFAB_PATHS_SM = { BUILDING_PREFAB_PATH_SM_00 };

    public static string BUILDING_PREFAB_PATH_IQALUIT_HALL = @"Prefabs/Buildings/Building_Iqaluit_Hall";
    public static string BUILDING_PREFAB_PATH_IQALUIT_AIRPORT = @"Prefabs/Buildings/Building_Iqaluit_Airport";
    public static string BUILDING_PREFAB_PATH_IQALUIT_NAKASUK = @"Prefabs/Buildings/Building_Iqaluit_Nakasuk";

    public static string BUILDING_PREFAB_PATH_OTTAWA_HALL = @"Prefabs/Buildings/Building_Ottawa_Hall";
    public static string BUILDING_PREFAB_PATH_OTTAWA_PARLIAMENT = @"Prefabs/Buildings/Building_Ottawa_Parliament";
    public static string BUILDING_PREFAB_PATH_OTTAWA_RIVER = @"Prefabs/Buildings/Building_Ottawa_River";

    public static string BUILDING_PREFAB_PATH_VANCOUVER_CP = @"Prefabs/Buildings/Building_Vancouver_CP";
    public static string BUILDING_PREFAB_PATH_VANCOUVER_HALL = @"Prefabs/Buildings/Building_Vancouver_Hall";
    public static string BUILDING_PREFAB_PATH_VANCOUVER_UBC = @"Prefabs/Buildings/Building_Vancouver_UBC";

    public static string TASK_TRAY_PREFAB_PATH = @"Prefabs/Tasks/TaskTray";
    public static string TASK_TRAY_SINGLE_PREFAB_PATH = @"Prefabs/Tasks/TaskTraySingle";
    public static string MEEPLE_PREFAB_PATH = @"Prefabs/MeepleController";
    
    public static string CITY_JSON_PATH = Application.dataPath + @"/Resources/Data/starting_cities";
    public const string SAVE_JSON_PATH = @"/Resources/Data/saved_games";
    #endregion

    public enum BUILDING_TYPE
    {
        SMALL,
        MEDIUM,
        IQALUIT_HALL,
        IQALUIT_AIRPORT,
        IQALUIT_NAKASUK,
        OTTAWA_HALL,
        OTTAWA_PARLIAMENT,
        OTTAWA_RIVER,
        VANCOUVER_HALL,
        VANCOUVER_UBC,
        VANCOUVER_CANADA_PLACE,
        NUMELEMENTS
    };

    #region Names
    public const string FOOD_RESOURCE_NAME = "Food";
    public const string IQALUIT_CITY_NAME = "Iqaluit";
    public const string OTTAWA_CITY_NAME = "Ottawa";
    public const string VANCOUVER_CITY_NAME = "Vancouver";
    public static readonly string[] SEASON_DISPLAY_NAMES = {"Spring", "Summer", "Fall", "Winter"};
    #endregion

    public enum EVT_TYPE
    {
        CHOICE,
        MOD_RESOURCE
    };

    public static string EVT_CHOICE_EVENTS_PATH = Application.dataPath + @"/Resources/Data/Events/choice-events.json";
    public static string EVT_MOD_RESOURCE_EVENTS_PATH = Application.dataPath + @"/Resources/Data/Events/modify-resource-events.json";


    
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

