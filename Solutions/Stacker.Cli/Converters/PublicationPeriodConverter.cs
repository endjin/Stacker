// <copyright file="PublicationPeriodConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System;
using NodaTime;
using NodaTime.Calendars;
using Stacker.Cli.Domain.Publication;

namespace Stacker.Cli.Converters;

public class PublicationPeriodConverter
{
    public DateInterval Convert(PublicationPeriod publicationPeriod)
    {
        IWeekYearRule rule = WeekYearRules.Iso;
        var today = LocalDate.FromDateTime(DateTime.Today);
        int weekNumber = rule.GetWeekOfWeekYear(today);

        switch (publicationPeriod)
        {
            case PublicationPeriod.ThisWeek:
                LocalDate startOfThisWeek = LocalDate.FromWeekYearWeekAndDay(today.Year, weekNumber, IsoDayOfWeek.Monday);
                return new(startOfThisWeek, LocalDate.FromDateTime(DateTime.Today));
            case PublicationPeriod.LastWeek:
                LocalDate startOfLastWeek = LocalDate.FromWeekYearWeekAndDay(today.Year, rule.GetWeekOfWeekYear(today.PlusWeeks(-1)), IsoDayOfWeek.Monday);
                return new(startOfLastWeek, startOfLastWeek.PlusWeeks(1).PlusDays(-1));
            case PublicationPeriod.ThisMonth:
                var startOfThisMonth = LocalDate.FromDateTime(new(today.Year, today.Month, 1));
                return new(startOfThisMonth, today);
            case PublicationPeriod.LastMonth:
                LocalDate startOfLastMonth = LocalDate.FromDateTime(new(today.Year, today.Month, 1)).PlusMonths(-1);
                var endOfLastMonth = new LocalDate(startOfLastMonth.Year, startOfLastMonth.Month, startOfLastMonth.Calendar.GetDaysInMonth(startOfLastMonth.Year, startOfLastMonth.Month));
                return new(startOfLastMonth, endOfLastMonth);
            case PublicationPeriod.ThisYear:
                var startOfThisYear = LocalDate.FromDateTime(new(today.Year, 1, 1));
                return new(startOfThisYear, today);
            case PublicationPeriod.LastYear:
                LocalDate startOfLastYear = LocalDate.FromDateTime(new(today.Year, 1, 1)).PlusYears(-1);
                LocalDate endOfLastYear = LocalDate.FromDateTime(new(today.Year, 12, 31)).PlusYears(-1);
                return new(startOfLastYear, endOfLastYear);
            default:
                throw new ArgumentOutOfRangeException(nameof(publicationPeriod), publicationPeriod, null);
        }
    }
}