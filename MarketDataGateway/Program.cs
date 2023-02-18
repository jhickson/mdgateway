using MarketDataGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarketDataGatewayServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();
