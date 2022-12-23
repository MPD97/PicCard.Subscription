namespace PicCard.Subscription.Api.Controllers;

[Route("/")]
[ApiController]
public class InfoController : ControllerBase
{
    private readonly DaprClient _daprClient;

    public InfoController(DaprClient daprClient)
    {
        this._daprClient = daprClient;
    }


    [HttpGet]
    public async Task<ServiceInfo> Index()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";
        const string storeName = "subscription_store";
        const string key = "started_at_utc";
        var appName = AppDomain.CurrentDomain.FriendlyName;

        var startedAtUtc = await _daprClient.GetStateAsync<DateTime>(storeName, key);
        return new ServiceInfo(appName, environment, startedAtUtc);
    }
}

public record ServiceInfo(string Name, string Environment, DateTime StartedAtUtc);