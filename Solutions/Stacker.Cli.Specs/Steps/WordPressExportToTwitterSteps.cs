using Reqnroll;

namespace Stacker.Cli.Specs.Steps;

[Binding]
public class WordPressExportToTwitterSteps
{
    private readonly ScenarioContext scenarioContext;

    public WordPressExportToTwitterSteps(ScenarioContext scenarioContext)
    {
        this.scenarioContext = scenarioContext;
    }
}