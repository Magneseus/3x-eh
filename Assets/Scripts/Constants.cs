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
    
    #region Task Constants
    public static float TASK_MIN_STRUCTURAL_DMG = 0.0f;
    public static float TASK_MAX_STRUCTURAL_DMG = 1.0f;
    public static float TASK_MIN_FUNGAL_DMG = 0.0f;
    public static float TASK_MAX_FUNGAL_DMG = 1.0f;
    #endregion
}
