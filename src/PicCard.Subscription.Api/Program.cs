using PicCard.Subscription.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddDapr();
builder.AddServices();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();
app.UseApiPathBase(builder);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "unknown";

const string storeName = "subscription_store";
const string key = "started_at_utc";

var daprClient = new DaprClientBuilder().Build();
await daprClient.SaveStateAsync(storeName, key, DateTime.UtcNow);

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();