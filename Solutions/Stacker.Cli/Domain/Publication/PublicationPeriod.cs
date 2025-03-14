// <copyright file="PublicationPeriod.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Domain.Publication;

public enum PublicationPeriod
{
    /// <summary>
    /// PublicationPeriod has not been set.
    /// </summary>
    None,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates this week.
    /// </summary>
    ThisWeek,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates last week.
    /// </summary>
    LastWeek,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates this month.
    /// </summary>
    ThisMonth,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates last month.
    /// </summary>
    LastMonth,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates this year.
    /// </summary>
    ThisYear,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates within the last year.
    /// </summary>
    LastYear,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates within the last 5 years.
    /// </summary>
    LastFiveYears,

    /// <summary>
    /// Filter ContentItems with PublishedOn dates within the last 10 years.
    /// </summary>
    LastTenYears,
}