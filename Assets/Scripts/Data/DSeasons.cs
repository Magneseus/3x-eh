using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public static class DSeasons
{
    public enum _season { SPRING, SUMMER, FALL, WINTER, NUMELEMENTS};

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

    public static DateTime[] InitialSeasonSetup(DateTime[] seasonDates, DateTime currentDate, ref _season season)
    {
        DateTime[] seasonStartDates = InitialCurrentYearSeasonDates(seasonDates, currentDate);
        bool lateWinter = seasonDates[3] < seasonDates[0];
        season = InitialSeasonForCheck(lateWinter);
        seasonStartDates = InitialCalendarAdjust(currentDate, lateWinter, seasonStartDates, ref season);
        return seasonStartDates;
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
}
