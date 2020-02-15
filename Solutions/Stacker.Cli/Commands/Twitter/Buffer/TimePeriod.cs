// <copyright file="TimePeriod.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli.Commands.Twitter.Buffer
{
    public enum TimePeriod
    {
        /// <summary>
        /// TimePeriod has not been set.
        /// </summary>
        None,

        /// <summary>
        /// Publish Items with Publication Dates this week.
        /// </summary>
        ThisWeek,

        /// <summary>
        /// Publish Items with Publication Dates last week.
        /// </summary>
        LastWeek,

        /// <summary>
        /// Publish Items with Publication Dates this month.
        /// </summary>
        ThisMonth,

        /// <summary>
        /// Publish Items with Publication Dates last month.
        /// </summary>
        LastMonth,

        /// <summary>
        /// Publish Items with Publication Dates this year.
        /// </summary>
        ThisYear,

        /// <summary>
        /// Publish Items with Publication Dates last year.
        /// </summary>
        LastYear,
    }
}