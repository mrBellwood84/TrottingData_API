using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();

services.AddPersistence();

var app = builder.Build();

app.MapControllers();

app.Run();

