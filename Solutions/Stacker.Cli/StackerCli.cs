// <copyright file="StackerCli.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

using Stacker.Cli.Commands;
using Stacker.Cli.Extensions;
using Stacker.Cli.Infrastructure.Injection;

namespace Stacker.Cli;

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
    public static Task<int> Main(string[] args)
    {
        ServiceCollection registrations = new();
        registrations.ConfigureDependencies();

        TypeRegistrar registrar = new(registrations);
        CommandApp app = new(registrar);

        app.Configure(config =>
        {
            config.Settings.PropagateExceptions = false;
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("stacker");

            config.AddExample("bluesky", "buffer", "-c", """c:\temp\content.json""", "-n", "azureweekly");
            config.AddExample("bluesky", "buffer", "shuffle", "-n", "azureweekly");
            config.AddExample("mastodon", "buffer", "-c", """c:\temp\content.json""", "-n", "azureweekly");
            config.AddExample("mastodon", "buffer", "shuffle", "-n", "azureweekly");
            config.AddExample("linkedin", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin");
            config.AddExample("linkedin", "buffer", "shuffle", "-n", "endjin");
            config.AddExample("facebook", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin");
            config.AddExample("facebook", "buffer", "shuffle", "-n", "endjin");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--item-count", "10");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--publication-period", "ThisMonth");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--filter-by-tag", "MicrosoftFabric", "--what-if");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--filter-by-tag", "MicrosoftFabric", "--item-count", "10", "--randomise", "--what-if");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--from-date", "2023/06/01", "--to-date", "2023/06/30");
            config.AddExample("twitter", "buffer", "-c", """c:\temp\content.json""", "-n", "endjin", "--filter-by-tag", "PowerBI", "--from-date", "2023/06/01", "--to-date", "2023/06/30");
            config.AddExample("twitter", "buffer", "-h", """https://localhost/stacker-export.json""", "-n", "endjin", "--filter-by-tag", "MicrosoftFabric", "--what-if");
            config.AddExample("twitter", "buffer", "shuffle", "-n", "endjin");
            config.AddExample("environment", "init");
            config.AddExample("wordpress", "export", "markdown", "-w", """C:\temp\wordpress-export.xml""", "-o", """C:\Temp\Blog""");
            config.AddExample("wordpress", "export", "universal", "-w", """C:\temp\wordpress-export.xml""", "-o", """C:\Temp\Blog\export.json""");

            config.AddBranch("bluesky", process =>
            {
                process.SetDescription("Bluesky functionality.");
                process.AddCommand<BlueskyBufferCommand>("buffer")
                       .WithDescription("Uploads content to Buffer to be published via Bluesky");
            });

            config.AddBranch("facebook", process =>
            {
                process.SetDescription("Facebook functionality.");
                process.AddCommand<FacebookBufferCommand>("buffer")
                       .WithDescription("Uploads content to Buffer to be published via Facebook");
            });

            config.AddBranch("linkedin", process =>
            {
                process.SetDescription("LinkedIn functionality.");
                process.AddCommand<LinkedInBufferCommand>("buffer")
                       .WithDescription("Uploads content to Buffer to be published via LinkedIn");
            });

            config.AddBranch("mastodon", process =>
            {
                process.SetDescription("Mastodon functionality.");
                process.AddCommand<MastodonBufferCommand>("buffer")
                       .WithDescription("Uploads content to Buffer to be published via Mastodon");
            });

            config.AddBranch("twitter", process =>
            {
                process.SetDescription("Twitter functionality.");
                process.AddCommand<TwitterBufferCommand>("buffer")
                       .WithDescription("Uploads content to Buffer to be published via Twitter");
            });

            config.AddBranch("environment", process =>
            {
                process.SetDescription("Manipulate the stacker environment.");
                process.AddCommand<EnvironmentInitCommand>("init")
                       .WithDescription("Initializes the stacker environment.");
            });

            config.AddBranch("wordpress", process =>
            {
                process.SetDescription("WordPress functionality.");
                process.AddBranch("export", export =>
                {
                    export.SetDescription("Export functionality.");
                    export.AddCommand<WordPressExportMarkdownCommand>("markdown")
                          .WithDescription("Convert WordPress export files into a markdown format.");

                    export.AddCommand<WordPressExportUniversalCommand>("universal")
                          .WithDescription("Convert WordPress export files into a universal format.");
                });
            });

            config.ValidateExamples();
        });

        return app.RunAsync(args);
    }
}