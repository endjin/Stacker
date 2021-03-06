# Stacker
A .NET Global Tool for automating marketing content across social channels. It supports extracting content from WordPress Export files and then republishing via Buffer to Twitter & LinkedIn, with automatically generated leader copy.

`stacker` is built using Microsoft's `System.CommandLine` [libraries](https://github.com/dotnet/command-line-api). These packages, while still marked as experimental, are seeing lots of real-world usage, including tools such as [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). 

A useful blog post for understanding `System.CommandLine` is [Radu Matei's](https://twitter.com/matei_radu) blog post "[Building self-contained, single executable .NET Core 3 CLI tools](https://radu-matei.com/blog/self-contained-dotnet-cli/)".

## Prerequisites

`stacker` used Pandoc to convert from wordpress to markdown. You will need to install [Pandoc](https://pandoc.org/installing.html) and add it to the `PATH`.

## dotnet global tools

`stacker` is a [.NET global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools), which means once installed, it's available on the PATH of your machine. 

To list all the global tools installed on your machine, open a command prompt and type:

`dotnet tool list -g`

To install the `stacker` global tool use the following command:

`dotnet tool install -g stacker`

To install a specific version, use:

`dotnet tool install -g stacker --version <version-number>`

To update to the latest version of the tool, use:

`dotnet tool update -g stacker`

To uninstall the tool, use:

`dotnet tool uninstall -g stacker`

## dotnet-suggest

`stacker` supports [dotnet suggest](https://github.com/dotnet/command-line-api/wiki/dotnet-suggest), for tab based auto completion.

To install dotnet suggest:

`dotnet tool install -g dotnet-suggest`

Next check if you have a PowerShell profile configured, by opening a PowerShell prompt and typing the following:

`echo $profile`

You should see something like:

`$ENV:USERPROFILE\Documents\PowerShell\Microsoft.PowerShell_profile.ps1`

If you don't see such a file run the following command:

`Invoke-WebRequest -Uri https://raw.githubusercontent.com/dotnet/command-line-api/master/src/System.CommandLine.Suggest/dotnet-suggest-shim.ps1 -OutFile $profile`

Otherwise, copy the contents of the file above and paste it into your pre-existing profile.

## Commands

Once you have `dotnet-suggest` installed, you can use `stacker` then TAB to explore the available commands. Here is a detailed list of the available commands:

`stacker environment` - Manipulate the stacker environment. Root command for environment operations. Will list available sub-commands.

`stacker environment init` - Initialises the `stacker` environment. Write's a default `StackerSettings.json` file to `%%UserProfile%%\AppData\Roaming\endjin\stacker\configuration`

`stacker wordpress` - Interact with WordPress. Root command for WordPress operations. Will list available sub-commands.

`stacker wordpress export universal` - Exports blog posts from WordPress into a reusable format suitable for publishing across social channels.

`wordpress export markdown` - Exports blog posts from WordPress and converts them into Markdown. Various clean up routes are also run.

### Buffer commands

`stacker twitter buffer <CONTENT-PATH> <ACCOUNT>` - Upload content items into buffer for the specified Twitter profile.

`stacker linkedin buffer <CONTENT-PATH> <ACCOUNT>` - Upload content items into buffer for the specified LinkedIn profile.

`stacker facebook buffer <CONTENT-PATH> <ACCOUNT>` - Upload content items into buffer for the specified Facebook profile.

The `buffer` command also takes the following options to filter the content items to be buffered.

```
Options:
  --item-count <item-count>                                                              Number of content items to buffer. If omitted all content is buffered.
  --publication-period <LastMonth|LastWeek|LastYear|None|ThisMonth|ThisWeek|ThisYear>    Publication period to filter content items by. If specified --from-date and --to-date are ignored.
  --from-date <from-date>                                                                Include content items published on, or after this date. If omitted DateTime.MinValue is used.
  --to-date <to-date>                                                                    Include content items published on, or before this date. If omitted DateTime.MaxValue is used.
```

## System Details

An application profile folder is created in:

`%%UserProfile%%\AppData\Roaming\endjin\stacker`

Configuration is stored in:

`configuration\`

## CI / CD

The project is [hosted on Azure DevOps](https://dev.azure.com/endjin-labs/stacker) under the `endjin-labs` org.

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

This project is sponsored by [endjin](https://endjin.com), a UK based Microsoft Gold Partner for Cloud Platform, Data Platform, Data Analytics, DevOps, a Power BI Partner, and .NET Foundation Corporate Sponsor.

We help small teams achieve big things.

For more information about our products and services, or for commercial support of this project, please [contact us](https://endjin.com/contact-us). 

We produce two free weekly newsletters; [Azure Weekly](https://azureweekly.info) for all things about the Microsoft Azure Platform, and [Power BI Weekly](https://powerbiweekly.info).

Keep up with everything that's going on at endjin via our [blog](https://blogs.endjin.com/), follow us on [Twitter](https://twitter.com/endjin), or [LinkedIn](https://www.linkedin.com/company/1671851/).

Our other Open Source projects can be found on [our website](https://endjin.com/open-source)

## Code of conduct

This project has adopted a code of conduct adapted from the [Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior in our community. This code of conduct has been [adopted by many other projects](http://contributor-covenant.org/adopters/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;](&#109;&#097;&#105;&#108;&#116;&#111;:&#104;&#101;&#108;&#108;&#111;&#064;&#101;&#110;&#100;&#106;&#105;&#110;&#046;&#099;&#111;&#109;) with any additional questions or comments.

## IP Maturity Matrix (IMM)

The [IP Maturity Matrix](https://github.com/endjin/Endjin.Ip.Maturity.Matrix) is endjin's IP quality framework; it defines a [configurable set of rules](https://github.com/endjin/Endjin.Ip.Maturity.Matrix.RuleDefinitions), which are committed into the [root of a repo](imm.yaml), and a [Azure Function HttpTrigger](https://github.com/endjin/Endjin.Ip.Maturity.Matrix/tree/master/Solutions/Endjin.Ip.Maturity.Matrix.Host) which can evaluate the ruleset, and render an svg badge for display in repo's `readme.md`.

This approach is based on our 10+ years experience of delivering complex, high performance, bleeding-edge projects, and due diligence assessments of 3rd party systems. For detailed information about the ruleset see the [IP Maturity Matrix repo](https://github.com/endjin/Endjin.Ip.Maturity.Matrix).

## IMM for stacker

[![Shared Engineering Standards](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?nocache=true)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/74e29f9b-6dca-4161-8fdd-b468a1eb185d?cache=false)

[![Coding Standards](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/f6f6490f-9493-4dc3-a674-15584fa951d8?cache=false)

[![Executable Specifications](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/bb49fb94-6ab5-40c3-a6da-dfd2e9bc4b00?cache=false)

[![Code Coverage](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/0449cadc-0078-4094-b019-520d75cc6cbb?cache=false)

[![Benchmarks](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/64ed80dc-d354-45a9-9a56-c32437306afa?cache=false)

[![Reference Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/2a7fc206-d578-41b0-85f6-a28b6b0fec5f?cache=false)

[![Design & Implementation Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/f026d5a2-ce1a-4e04-af15-5a35792b164b?cache=false)

[![How-to Documentation](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/145f2e3d-bb05-4ced-989b-7fb218fc6705?cache=false)

[![Date of Last IP Review](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/da4ed776-0365-4d8a-a297-c4e91a14d646?cache=false)

[![Framework Version](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/6c0402b3-f0e3-4bd7-83fe-04bb6dca7924?cache=false)

[![Associated Work Items](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/79b8ff50-7378-4f29-b07c-bcd80746bfd4?cache=false)

[![Source Code Availability](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/30e1b40b-b27d-4631-b38d-3172426593ca?cache=false)

[![License](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/d96b5bdc-62c7-47b6-bcc4-de31127c08b7?cache=false)

[![Production Use](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/87ee2c3e-b17a-4939-b969-2c9c034d05d7?cache=false)

[![Insights](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/71a02488-2dc9-4d25-94fa-8c2346169f8b?cache=false)

[![Packaging](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/547fd9f5-9caf-449f-82d9-4fba9e7ce13a?cache=false)

[![Deployment](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)](https://endimmfuncdev.azurewebsites.net/api/imm/github/endjin/Stacker/rule/edea4593-d2dd-485b-bc1b-aaaf18f098f9?cache=false)