using NUnit.Framework;
using System.Collections.Generic;
using Assets.Editor.UnitTests;
using System;

public class SeasonsTests
{
    private DateTime currentDate = new DateTime(2017, 1, 1);
    private DateTime[] defaultSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 12, 1) };
    private DateTime[] defaultDeadOfWinterDates = { new DateTime(2017, 1, 7), new DateTime(2017, 1, 21) };
    private DateTime[] lateWinterSeasonStartDates = { new DateTime(2017, 4, 1), new DateTime(2017, 6, 1), new DateTime(2017, 8, 1), new DateTime(2017, 1, 5) };
    private DateTime[] zeroYearSeasonStartDates = { new DateTime(1000, 4, 1), new DateTime(1000, 6, 1), new DateTime(1000, 8, 1), new DateTime(1000, 12, 1) };
    private DateTime[] zeroYearLateWinterSeasonStartDates = { new DateTime(1000, 4, 1), new DateTime(1000, 6, 1), new DateTime(1000, 8, 1), new DateTime(1000, 1, 5) };


    [TearDown]
    public void TearDown()
    {
        Mock.TearDown();
    }

    [Test]
    public void SeasonsTestsSimplePasses()
    {
        Assert.Pass();
    }

    [Test]
    public void NextSeasonReturns()
    {
        for (int i=0; i<(int)DSeasons._season.NUMELEMENTS; i++)
        {
            int x = i+1;
            if (x == (int)DSeasons._season.NUMELEMENTS)
                x = 0;
            Assert.That(DSeasons.NextSeason((DSeasons._season)i), Is.EqualTo((DSeasons._season)x));
        }
    }

    [Test]
    public void PrevSeasonReturns()
    {
        for (int i = 0; i < (int)DSeasons._season.NUMELEMENTS; i++)
        {
            int x = i - 1;
            if (x == -1)
                x = (int)DSeasons._season.NUMELEMENTS - 1;
            Assert.That(DSeasons.PreviousSeason((DSeasons._season)i), Is.EqualTo((DSeasons._season)x));
        }
    }

    [Test]
    public void AdvanceSeasonDateOneYear()
    {
        DateTime date = new DateTime(3049, 1, 1);
        Assert.That(new DateTime(3050, 1, 1), Is.EqualTo(DSeasons.SetNextSeasonDate(date)));
    }

    [Test]
    public void DateUpdatesSeason()
    {
        DateTime[] startDates = new DateTime[defaultSeasonStartDates.Length];
        Array.Copy(defaultSeasonStartDates, startDates, defaultSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.WINTER;

        startDates = DSeasons.UpdateSeasonStatus(startDates, currentDate.AddMonths(6), ref season);
        Assert.That(season, Is.EqualTo(DSeasons._season.SUMMER));
    }

    [Test]
    public void DateUpdatesSeasonStartDates()
    {
        DateTime[] startDates = defaultSeasonStartDates;
        DSeasons._season season = DSeasons._season.WINTER;

        startDates = DSeasons.UpdateSeasonStatus(startDates, currentDate.AddMonths(6), ref season);
        Assert.IsFalse(startDates[0] == defaultSeasonStartDates[0].AddYears(1));
        Assert.IsFalse(startDates[1] == defaultSeasonStartDates[1].AddYears(1));
    }

    [Test]
    public void SeasonToStartLoopFrom()
    {
        Assert.That(DSeasons.InitialSeasonForCheck(false), Is.EqualTo(DSeasons._season.WINTER));
        Assert.That(DSeasons.InitialSeasonForCheck(true), Is.EqualTo(DSeasons._season.FALL));
    }

    [Test]
    public void SetsSeasonDatesToCurrentYear()
    {
        DateTime[] startDates = zeroYearSeasonStartDates;

        startDates = DSeasons.InitialCurrentYearSeasonDates(startDates, currentDate);
        for (int i = 0; i < startDates.Length; i++)
            Assert.That(startDates[i], Is.EqualTo(new DateTime(currentDate.Year, zeroYearSeasonStartDates[i].Month, zeroYearSeasonStartDates[i].Day)));
    }

    [Test]
    public void AdjustsSeasonBasedOnDate()
    {
        DateTime[] startDates = defaultSeasonStartDates;
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = startDates[1].AddDays(1);

        startDates = DSeasons.InitialCalendarAdjust(date, false, startDates, ref season);

        Assert.That(season, Is.EqualTo(DSeasons._season.SUMMER));
    }

    [Test]
    public void AdjustsStartDatesBasedOnDate()
    {
        DateTime[] startDates = new DateTime[defaultSeasonStartDates.Length];
        Array.Copy(defaultSeasonStartDates, startDates, defaultSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = startDates[1].AddDays(1);

        startDates = DSeasons.InitialCalendarAdjust(date, false, startDates, ref season);

        Assert.IsTrue(startDates[0] == defaultSeasonStartDates[0].AddYears(1));
        Assert.IsTrue(startDates[1] == defaultSeasonStartDates[1].AddYears(1));
        Assert.IsTrue(startDates[2] == defaultSeasonStartDates[2]);
        Assert.IsTrue(startDates[3] == defaultSeasonStartDates[3]);
    }

    [Test]
    public void AdjustsSeasonBasedOnDateWithLateWinter()
    {
        DateTime[] startDates = new DateTime[lateWinterSeasonStartDates.Length];
        Array.Copy(lateWinterSeasonStartDates, startDates, lateWinterSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = startDates[1].AddDays(1);

        startDates = DSeasons.InitialCalendarAdjust(date, true, startDates, ref season);

        Assert.That(season, Is.EqualTo(DSeasons._season.SUMMER));
    }

    [Test]
    public void AdjustsSeasonStartsBasedOnDateWithLateWinter()
    {
        DateTime[] startDates = new DateTime[lateWinterSeasonStartDates.Length];
        Array.Copy(lateWinterSeasonStartDates, startDates, lateWinterSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = startDates[1].AddDays(1);

        startDates = DSeasons.InitialCalendarAdjust(date, true, startDates, ref season);

        Assert.IsTrue(startDates[0] == lateWinterSeasonStartDates[0].AddYears(1));
        Assert.IsTrue(startDates[1] == lateWinterSeasonStartDates[1].AddYears(1));
        Assert.IsTrue(startDates[2] == lateWinterSeasonStartDates[2]);
        Assert.IsTrue(startDates[3] == lateWinterSeasonStartDates[3].AddYears(1));
    }

    [Test]
    public void InitialSetupForSeason()
    {
        DateTime[] startDates = zeroYearSeasonStartDates;
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = new DateTime(currentDate.Year, startDates[1].Month, startDates[1].AddDays(1).Day);
        DateTime[] deadOfWinter = new DateTime[2];
        Array.Copy(defaultDeadOfWinterDates, deadOfWinter, defaultDeadOfWinterDates.Length);

        startDates = DSeasons.InitialSeasonSetup(startDates, date, ref season, ref deadOfWinter);

        Assert.That(season, Is.EqualTo(DSeasons._season.SUMMER));
    }

    [Test]
    public void InitialSetupForDates()
    {
        DateTime[] startDates = new DateTime[zeroYearSeasonStartDates.Length];
        Array.Copy(zeroYearSeasonStartDates, startDates, zeroYearSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = new DateTime(currentDate.Year, startDates[1].Month, startDates[1].AddDays(1).Day);
        DateTime[] deadOfWinter = new DateTime[2];
        Array.Copy(defaultDeadOfWinterDates, deadOfWinter, defaultDeadOfWinterDates.Length);

        DateTime[] compareDates = new DateTime[zeroYearSeasonStartDates.Length];
        Array.Copy(zeroYearSeasonStartDates, startDates, zeroYearSeasonStartDates.Length);
        for(int i=0; i<compareDates.Length; i++)
            compareDates[i] = new DateTime(currentDate.Year, zeroYearSeasonStartDates[i].Month, zeroYearSeasonStartDates[i].Day);

        startDates = DSeasons.InitialSeasonSetup(startDates, date, ref season, ref deadOfWinter);

        Assert.IsTrue(startDates[0] == compareDates[0].AddYears(1));
        Assert.IsTrue(startDates[1] == compareDates[1].AddYears(1));
        Assert.IsTrue(startDates[2] == compareDates[2]);
        Assert.IsTrue(startDates[3] == compareDates[3]);
    }

    [Test]
    public void InitialSetupForSeasonWithLateWinter()
    {
        DateTime[] startDates = zeroYearLateWinterSeasonStartDates;
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = new DateTime(currentDate.Year, startDates[1].Month, startDates[1].AddDays(1).Day);
        DateTime[] deadOfWinter = new DateTime[2];
        Array.Copy(defaultDeadOfWinterDates, deadOfWinter, defaultDeadOfWinterDates.Length);

        startDates = DSeasons.InitialSeasonSetup(startDates, date, ref season, ref deadOfWinter);

        Assert.That(season, Is.EqualTo(DSeasons._season.SUMMER));
    }

    [Test]
    public void InitialSetupForDatesWithLateWinter()
    {
        DateTime[] startDates = new DateTime[zeroYearLateWinterSeasonStartDates.Length];
        Array.Copy(zeroYearLateWinterSeasonStartDates, startDates, zeroYearLateWinterSeasonStartDates.Length);
        DSeasons._season season = DSeasons._season.NUMELEMENTS;
        DateTime date = new DateTime(currentDate.Year, startDates[1].Month, startDates[1].AddDays(1).Day);
        DateTime[] deadOfWinter = new DateTime[2];
        Array.Copy(defaultDeadOfWinterDates, deadOfWinter, defaultDeadOfWinterDates.Length);

        DateTime[] compareDates = new DateTime[zeroYearLateWinterSeasonStartDates.Length];
        Array.Copy(zeroYearLateWinterSeasonStartDates, compareDates, zeroYearLateWinterSeasonStartDates.Length);
        for (int i = 0; i < compareDates.Length; i++)
            compareDates[i] = new DateTime(currentDate.Year, zeroYearLateWinterSeasonStartDates[i].Month, zeroYearLateWinterSeasonStartDates[i].Day);

        startDates = DSeasons.InitialSeasonSetup(startDates, date, ref season, ref deadOfWinter);

        Assert.IsTrue(startDates[0] == compareDates[0].AddYears(1));
        Assert.IsTrue(startDates[1] == compareDates[1].AddYears(1));
        Assert.IsTrue(startDates[2] == compareDates[2]);
        Assert.IsTrue(startDates[3] == compareDates[3].AddYears(1));
    }
}