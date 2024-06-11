using Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDatabase(builder.Configuration);

builder.Services.AddDependencyInjections();

builder.Services.ConfigureSwagger();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

ApiConfig.ConfigureApi(app);

app.Run();
