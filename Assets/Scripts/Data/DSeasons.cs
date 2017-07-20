using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public static class DSeasons
{
    public enum _season { SPRING, SUMMER, FALL, WINTER, NUMELEMENTS };
    private static int[,] defaultStartDates = { { 2017, 4, 1 },
                                                { 2017, 6, 1 },
                                                { 2017, 9, 1 },
                                                { 2017, 12, 1 }};
    private static int[,] defaultDeadOfWinterDates = { { 2017, 1, 7 },
                                                { 2017, 1, 21 } };

    public static float[] modFoodConsumption = {1f, 1f, 1f, 1.5f };
    public static float[] modFoodProduction = {1f, 1f, 1f, 0.4f };
    public static float[] modRepairStructureSpeed = { 1f, 1f, 1f, 0.5f };
    public static float[] modRepairFungusSpeed = { 1f, 0.5f, 1f, 2f };
    public static float[] changeFungusSlots = { 1f, 1.25f, 1f, 0.8f };
    public static float[] changeStructureDamageSlots = { 1f, 1f, 1f, 1.25f };
    public static float[] modBuildingInfection = { 1f, 1.5f, 1f, 0.5f };
    public static float reduceHealthExploringWinter = -0.1f;

    public static _season NextSeason(_season currentSeason)
    {
        return (_season)(((int)currentSeason + 1) % (int)_season.NUMELEMENTS);
    }
    
    public static _season PreviousSeason(_season currentSeason)
    {
        int prevSeasonNum = (int)currentSeason - 1;
        if (prevSeasonNum < 0)
            return (_season)((int)_season.NUMELEMENTS - 1);
        return (_season)prevSeasonNum;
    }

    public static DateTime[] UpdateSeasonStatus(DateTime[] seasonDates, DateTime currentDate, ref _season season)
    {
        DateTime[] result = seasonDates;
        for (int i = 0; i < (int)_season.NUMELEMENTS; i++)
            if (NextSeasonBegun(result, currentDate, season))
            {
                season = NextSeason(season);
                result[(int)season] = SetNextSeasonDate(result[(int)season]);
            }
            else
                break;
        return result;
    }

    public static DateTime SetNextSeasonDate(DateTime currentStartDate)
    {
        return currentStartDate.AddYears(1);
    }

    public static bool NextSeasonBegun(DateTime[] seasonDates, DateTime currentDate, _season currentSeason)
    {
        return currentDate > seasonDates[(int)NextSeason(currentSeason)];
    }

    public static DateTime[] InitialSeasonSetup(DateTime[] seasonDates, DateTime currentDate, ref _season season, ref DateTime[] deadWinterDates)
    {
        DateTime[] seasonStartDates = InitialCurrentYearSeasonDates(seasonDates, currentDate);
        bool lateWinter = seasonDates[3] < seasonDates[0];
        season = InitialSeasonForCheck(lateWinter);
        seasonStartDates = InitialCalendarAdjust(currentDate, lateWinter, seasonStartDates, ref season);
        deadWinterDates = InitialDeadOfWinterDates(deadWinterDates, currentDate);
        return seasonStartDates;
    }

    public static DateTime[] InitialDeadOfWinterDates(DateTime[] deadWinterDates, DateTime currentDate)
    {
        for (int i = 0; i < deadWinterDates.Length; i++)
            if (currentDate > deadWinterDates[i])
                deadWinterDates[i].AddYears(1);
        return deadWinterDates;
    }

    public static DateTime[] InitialCurrentYearSeasonDates(DateTime[] seasonDates, DateTime currentDate)
    {
        DateTime[] seasonStartDates = new DateTime[4];
        for (int i = 0; i < seasonStartDates.Length; i++)
            seasonStartDates[i] = new DateTime(currentDate.Year, seasonDates[i].Month, seasonDates[i].Day);
        return seasonStartDates;
    }

    public static _season InitialSeasonForCheck(bool hasLateWinter)
    {
        if (hasLateWinter)
            return _season.FALL;
        else
            return _season.WINTER;
    }

    public static DateTime[] InitialCalendarAdjust(DateTime currentDate, bool lateWinter, DateTime[] seasonStartDates, ref _season season)
    {
        DateTime[] results = seasonStartDates;
        int start = lateWinter ? 3 : 0;
        int end = lateWinter ? 7 : 4;
        for (int i = start; i < end; i++)
        {
            int element = i % (int)DSeasons._season.NUMELEMENTS;
            if (currentDate > seasonStartDates[element])
            {
                season = (DSeasons._season)element;
                results[element] = results[element].AddYears(1);
            }
        }
        return results;
    }

    public static DateTime[] DefaultStartDates()
    {

        DateTime[] results = {new DateTime(defaultStartDates[0,0], defaultStartDates[0,1], defaultStartDates[0,2]),
            new DateTime(defaultStartDates[1,0], defaultStartDates[1,1], defaultStartDates[1,2]),
            new DateTime(defaultStartDates[2,0], defaultStartDates[2,1], defaultStartDates[2,2]),
            new DateTime(defaultStartDates[3,0], defaultStartDates[3,1], defaultStartDates[3,2])};
        return results;
    }

    public static DateTime[] DefaultDeadOfWinterDates()
    {

        DateTime[] results = {new DateTime(defaultDeadOfWinterDates[0,0], defaultDeadOfWinterDates[0,1], defaultDeadOfWinterDates[0,2]),
            new DateTime(defaultDeadOfWinterDates[1,0], defaultDeadOfWinterDates[1,1], defaultDeadOfWinterDates[1,2]) };
        return results;
    }
}
