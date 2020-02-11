namespace Stacker.Cli.Specs.Steps
{
    using Endjin.Stacker.Cli;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using TechTalk.SpecFlow;

    [Binding]
    public class WordPressExportToTwitterSteps
    {
        private readonly ScenarioContext scenarioContext;

        public WordPressExportToTwitterSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
    }
}