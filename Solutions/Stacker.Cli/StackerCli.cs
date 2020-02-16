// <copyright file="StackerCli.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Stacker.Cli
{
    using System.CommandLine.Builder;
    using System.CommandLine.Invocation;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Stacker.Cli.Commands;
    using Stacker.Cli.Contracts.Commands;
    using Stacker.Cli.Extensions;

    /// <summary>
    /// A CLI tool for automating marketing activities.
    /// </summary>
    public static class StackerCli
    {
        /// <summary>
        /// A Marketing Automation .NET Global Tool.
        /// </summary>
        /// <param name="args">Command Line Switches.</param>
        /// <returns>Exit Code.</returns>
        public static async Task<int> Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureDependencies();

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var cmd = new CommandLineBuilder()
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<FacebookCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<LinkedInCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<TwitterCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<WordPressCommandFactory>>().Create())
                .AddCommand(serviceProvider.GetRequiredService<ICommandFactory<EnvironmentCommandFactory>>().Create())
                .UseDefaults()
                .Build();

            return await cmd.InvokeAsync(args).ConfigureAwait(false);
        }
    }
}