// <copyright file="TimePeriodConverter.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Twitter.Buffer
{
    using System;
    using NodaTime;
    using NodaTime.Calendars;

    public class TimePeriodConverter
    {
        public DateInterval Convert(TimePeriod timePeriod)
        {
            IWeekYearRule rule = WeekYearRules.Iso;
            var today = LocalDate.FromDateTime(DateTime.Today);
            var weekNumber = rule.GetWeekOfWeekYear(today);

            switch (timePeriod)
            {
                case TimePeriod.ThisWeek:
                    LocalDate startOfThisWeek = LocalDate.FromWeekYearWeekAndDay(today.Year, weekNumber, IsoDayOfWeek.Monday);
                    return new DateInterval(startOfThisWeek, LocalDate.FromDateTime(DateTime.Today));
                case TimePeriod.LastWeek:
                    LocalDate startOfLastWeek = LocalDate.FromWeekYearWeekAndDay(today.Year, rule.GetWeekOfWeekYear(today.PlusWeeks(-1)), IsoDayOfWeek.Monday);
                    return new DateInterval(startOfLastWeek, startOfLastWeek.PlusWeeks(1).PlusDays(-1));
                case TimePeriod.ThisMonth:
                    var startOfThisMonth = LocalDate.FromDateTime(new DateTime(today.Year, today.Month, 1));
                    return new DateInterval(startOfThisMonth, today);
                case TimePeriod.LastMonth:
                    var startOfLastMonth = LocalDate.FromDateTime(new DateTime(today.Year, today.Month, 1)).PlusMonths(-1);
                    var endOfLastMonth = new LocalDate(startOfLastMonth.Year, startOfLastMonth.Month, startOfLastMonth.Calendar.GetDaysInMonth(startOfLastMonth.Year, startOfLastMonth.Month));
                    return new DateInterval(startOfLastMonth, endOfLastMonth);
                default:
                    throw new ArgumentOutOfRangeException(nameof(timePeriod), timePeriod, null);
            }
        }
    }
}