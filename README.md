# Stacker

A .NET Global Tool for automating marketing content across social channels. It supports extracting content from Vellum and then republishing via Buffer to Twitter, Mastodon, Facebook & LinkedIn, with automatically generated leader copy.

## Prerequisites

`stacker` uses Pandoc for WordPress export functionality, to convert from wordpress to markdown. You will need to install [Pandoc](https://pandoc.org/installing.html) and add it to the `PATH` or use `winget install JohnMacFarlane.Pandoc`.

## Installation

`stacker` is a [.NET global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools), which means once installed, it's available on the PATH of your machine. 

To list all the global tools installed on your machine, open a command prompt and type:

`dotnet tool list -g`

To install the `stacker` global tool use the following command:

`dotnet tool install -g stacker`

Then use `stacker environment init` to create a default `StackerSettings.json` file with placeholder values in `%%UserProfile%%\AppData\Roaming\endjin\stacker\configuration`.

To install a specific version, use:

`dotnet tool install -g stacker --version <version-number>`

To update to the latest version of the tool, use:

`dotnet tool update -g stacker`

To uninstall the tool, use:

`dotnet tool uninstall -g stacker`

## Commands

Here are some usage examples:

```PowerShell
USAGE:
    stacker [OPTIONS] <COMMAND>

EXAMPLES:
    stacker bluesky buffer create -c c:\temp\content.json -n azureweekly
    stacker bluesky buffer shuffle -n azureweekly
    stacker mastodon buffer create -c c:\temp\content.json -n azureweekly
    stacker mastodon buffer shuffle -n azureweekly
    stacker linkedin buffer create -c c:\temp\content.json -n endjin
    stacker linkedin buffer shuffle -n endjin
    stacker facebook buffer create -c c:\temp\content.json -n endjin
    stacker facebook buffer shuffle -n endjin
    stacker twitter buffer create -c c:\temp\content.json -n endjin
    stacker twitter buffer create -c c:\temp\content.json -n endjin --item-count 10
    stacker twitter buffer create -c c:\temp\content.json -n endjin --publication-period ThisMonth
    stacker twitter buffer create -c c:\temp\content.json -n endjin --filter-by-tag MicrosoftFabric --what-if
    stacker twitter buffer create -c c:\temp\content.json -n endjin --filter-by-tag MicrosoftFabric --item-count 10 --randomise --what-if
    stacker twitter buffer create -c c:\temp\content.json -n endjin --from-date 2023/06/01 --to-date 2023/06/30
    stacker twitter buffer create -c c:\temp\content.json -n endjin --filter-by-tag PowerBI --from-date 2023/06/01 --to-date 2023/06/30
    stacker twitter buffer create -h https://localhost/stacker-export.json -n endjin --filter-by-tag MicrosoftFabric --what-if
    stacker twitter buffer shuffle -n endjin
    stacker environment init
    stacker wordpress export markdown -w C:\temp\wordpress-export.xml -o C:\Temp\Blog
    stacker wordpress export universal -w C:\temp\wordpress-export.xml -o C:\Temp\Blog\export.json

OPTIONS:
    -h, --help    Prints help information

COMMANDS:
    bluesky        Bluesky functionality
    facebook       Facebook functionality
    linkedin       LinkedIn functionality
    mastodon       Mastodon functionality
    twitter        Twitter functionality
    environment    Manipulate the stacker environment
    wordpress      WordPress functionality
```

### Buffer commands

`stacker twitter buffer create` - Upload content items into buffer for the specified Twitter profile.

`stacker linkedin buffer create` - Upload content items into buffer for the specified LinkedIn profile.

`stacker facebook buffer create` - Upload content items into buffer for the specified Facebook profile.

The `buffer create` command also takes the following options to filter the content items to be buffered.

```
OPTIONS:
    -h, --help                  Prints help information
    -c, --content-file-path     Content file path
    -n, --profile-name          Buffer channel profile (e.g. twitter, linkedin, facebook) name (e.g. endjin, azureweekly, powerbiweekly)
    -g, --filter-by-tag         Tag to filter the content items by
    -i, --item-count            Number of content items to buffer. If omitted all content is buffered
    -p, --publication-period    Publication period to filter content items by. <LastMonth|LastWeek|LastYear|None|ThisMonth|ThisWeek|ThisYear> If specified --from-date and --to-date are ignored
    -f, --from-date             Include content items published on, or after this date. Use YYYY/MM/DD Format. If omitted DateTime.MinValue is used
    -t, --to-date               Include content items published on, or before this date. Use YYYY/MM/DD Format. If omitted DateTime.MaxValue is used
    -w, --what-if               See what the command would do without submitting the content to Buffer
```

You can also shuffle items already in the buffer queue (useful if you're inserting content into and existing queue and need to mix it up).

`stacker bluesky buffer shuffle` - Shuffle the content items in the buffer for the specified Bluesky profile.
`stacker twitter buffer shuffle` - Shuffle the content items in the buffer for the specified Twitter profile.
`stacker linkedin buffer shuffle` - Shuffle the content items in the buffer for the specified LinkedIn profile.
`stacker facebook buffer shuffle` - Shuffle the content items in the buffer for the specified Facebook profile.
`stacker mastodon buffer shuffle` - Shuffle the content items in the buffer for the specified Mastodon profile.

### WordPress commands

`stacker wordpress` - Interact with WordPress. Root command for WordPress operations. Will list available sub-commands.

`stacker wordpress export universal` - Exports blog posts from WordPress into a reusable format suitable for publishing across social channels.

`wordpress export markdown` - Exports blog posts from WordPress and converts them into Markdown. Various clean up routes are also run.

### Environment commands

`stacker environment` - Manipulate the stacker environment. Root command for environment operations. Will list available sub-commands.

`stacker environment init` - Initialises the `stacker` environment. Writes a default `StackerSettings.json` file with placeholder values to `%%UserProfile%%\AppData\Roaming\endjin\stacker\configuration`

## System Details

An application profile folder is created in:

`%%UserProfile%%\AppData\Roaming\endjin\stacker`

Configuration is stored in:

`%%UserProfile%%\AppData\Roaming\endjin\stacker\configuration\`

## DevOps

The project is [built using GitHub Actions](https://github.com/endjin/Stacker/actions) using [Endjin.RecommendedPractices.Build](https://www.powershellgallery.com/packages/Endjin.RecommendedPractices.Build/) and [Endjin.RecommendedPractices.GitHubActions](https://github.com/endjin/Endjin.RecommendedPractices.GitHubActions). Solution-level Engineering Practices are enforced using [Endjin.RecommendedPractices.NuGet](https://github.com/endjin/Endjin.RecommendedPractices.NuGet).

## Packages

The NuGet packages for the project, hosted on NuGet.org are:

- [stacker](https://www.nuget.org/packages/stacker)

## WordPress

Stacker supports filtering of blog posts via WordPress Custom Fields.

`stacker_promote` `true` | `false` - states whether Stacker should promote. If this field is missing, Stacker will automatically include it.

`stacker_promote_until` YYYY-MM-DD - states the date at which Stacker should stop including the post. This is for use cases around events which have no re-posting value.

Hastags are generated from WordPress Tags associated with each post.

The `Excerpt` custom field is used to provide the content summary when publishing to Facebook or LinkedIn.

## Licenses

[![GitHub license](https://img.shields.io/badge/License-Apache%202-blue.svg)](https://raw.githubusercontent.com/endjin/Stacker/master/LICENSE)

This project is available under the Apache 2.0 open source license.

For any licensing questions, please email [&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;](&#109;&#97;&#105;&#108;&#116;&#111;&#58;&#108;&#105;&#99;&#101;&#110;&#115;&#105;&#110;&#103;&#64;&#101;&#110;&#100;&#106;&#105;&#110;&#46;&#99;&#111;&#109;)

## Project Sponsor

This project is sponsored by [endjin](https://endjin.com), a fully remote UK based Technology Consultancy which specializes in Data, AI, DevOps & Cloud, and is a [.NET Foundation Corporate Sponsor](https://dotnetfoundation.org/membership/corporate-sponsorship).

> We help small teams achieve big things.

We produce two free weekly newsletters: 

 - [Azure Weekly](https://azureweekly.info) for all things about the Microsoft Azure Platform
 - [Power BI Weekly](https://powerbiweekly.info) for all things Power BI, Microsoft Fabric, and Azure Synapse Analytics

Keep up with everything that's going on at endjin via our [blog](https://endjin.com/blog), follow us on [Twitter](https://twitter.com/endjin), [YouTube](https://www.youtube.com/c/endjin) or [LinkedIn](https://www.linkedin.com/company/endjin).

We have become the maintainers of a number of popular .NET Open Source Projects:

- [Reactive Extensions for .NET](https://github.com/dotnet/reactive)
- [Reaqtor](https://github.com/reaqtive)
- [Argotic Syndication Framework](https://github.com/argotic-syndication-framework/)

And we have over 50 Open Source projects of our own, spread across the following GitHub Orgs:

- [endjin](https://github.com/endjin/)
- [Corvus](https://github.com/corvus-dotnet)
- [Menes](https://github.com/menes-dotnet)
- [Marain](https://github.com/marain-dotnet)
- [AIS.NET](https://github.com/ais-dotnet)

And the DevOps tooling we have created for managing all these projects is available on the [PowerShell Gallery](https://www.powershellgallery.com/profiles/endjin).

For more information about our consulting services, please [contact us](https://endjin.com/contact-us).

## Code of conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behaviour in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## IP Maturity Model (IMM)

The [IP Maturity Model](https://github.com/endjin/Endjin.Ip.Maturity.Matrix) is endjin's IP quality framework; it defines a [configurable set of rules](https://github.com/endjin/Endjin.Ip.Maturity.Matrix.RuleDefinitions), which are committed into the [root of a repo](imm.yaml), and a [Azure Function HttpTrigger](https://github.com/endjin/Endjin.Ip.Maturity.Matrix/tree/master/Solutions/Endjin.Ip.Maturity.Matrix.Host) which can evaluate the ruleset, and render an svg badge for display in repo's `readme.md`.

This approach is based on our 10+ years experience of delivering complex, high performance, bleeding-edge projects, and due diligence assessments of 3rd party systems. For detailed information about the ruleset see the [IP Maturity Model repo](https://github.com/endjin/Endjin.Ip.Maturity.Matrix).

## IMM for stacker

[![Shared Engineering Standards](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?nocache=true)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?cache=false)

[![Coding Standards](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)

[![Executable Specifications](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)

[![Code Coverage](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)

[![Benchmarks](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)

[![Reference Documentation](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)

[![Design & Implementation Documentation](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)

[![How-to Documentation](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)

[![Date of Last IP Review](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)

[![Framework Version](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)

[![Associated Work Items](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)

[![Source Code Availability](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)

[![License](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)

[![Production Use](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)

[![Insights](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)

[![Packaging](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)

[![Deployment](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)

[![OpenChain](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)](https://imm.endjin.com/api/imm/github/endjin/Stacker/rule/66efac1a-662c-40cf-b4ec-8b34c29e9fd7?cache=false)
