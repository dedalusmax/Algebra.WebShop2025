using Algebra.WebShop2025.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

var app = builder.Build();

app.Configure()
    .Run();
