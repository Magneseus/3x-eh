using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    //idk why this is hardcoded
    public static string OTTAWA_PREFAB_PATH = @"Prefabs/Cities/Ottawa";
    // should be like this (with the city sprite given the right sprite based on city name [ or even loaded from a path stored in the json itself])
    public static string CITY_PREFAB_PATH = @"Prefabs/Cities/City";
    public static string CITY_SPRITE_PATH = @"Sprites/";

    public static string BUILDING_PREFAB_PATH = @"Prefabs/Buildings/Building";
    public static string TASK_TRAY_PREFAB_PATH = @"Prefabs/Tasks/TaskTray";
    public static string TASK_TRAY_SINGLE_PREFAB_PATH = @"Prefabs/Tasks/TaskTraySingle";
    public static string MEEPLE_PREFAB_PATH = @"Prefabs/MeepleController";

    public static string CITY_JSON_PATH = @"Assets/Resources/Data";

    public static DateTime[] DEFAULT_SEASON_DATES = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };
    public static DateTime DEFAULT_DATE = new DateTime(2077, 7, 7);

    #region Task Constants
    public static float TEMP_REPAIR_AMOUNT = 0.2f;
    public static float DEFAULT_ASSESS_AMOUNT = 0.1f;

    public static float TASK_MIN_STRUCTURAL_DMG = 0.0f;
    public static float TASK_MAX_STRUCTURAL_DMG = 1.0f;
    public static float BUILDING_MIN_FUNGAL_DMG = 0.0f;
    public static float BUILDING_MAX_FUNGAL_DMG = 1.0f;
    public static float
    BUILDING_MERSON_INFECTION_WEIGHT = 0.25f;
    public static float TASK_MIN_FUNGAL_DMG = 0.0f;
    public static float TASK_MAX_FUNGAL_DMG = 1.0f;
    #endregion

    #region Merson Infection
    public static int MERSON_INFECTION_MIN = 0;
    public static int MERSON_INFECTION_MAX = 2;
    public static float MERSON_INFECTION_PROBABILITY = 0.05f;
    public static int MERSON_SPRING_FALL_INFECTION_MODIFIER = 1;
    public static float MERSON_INFECTION_TASK_MODIFIER = 0.5f;
    #endregion

    #region City Constants
    public static float CITY_MIN_HEALTH = 0.0f;
    public static float CITY_MAX_HEALTH = 1.0f;

    #endregion
}
