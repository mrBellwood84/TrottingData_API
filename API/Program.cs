using API.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// todo : API services not an extension yet, awaiting 
services.AddControllers();

// add services
services.AddEntityPolicies();
services.AddPersistence();

// crash on missing dependency added
builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var app = builder.Build();

// todo : configure pipelien in extension?
app.MapControllers();

app.Run();

