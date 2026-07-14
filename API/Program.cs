using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// todo : API services not an extension yet, awaiting 
services.AddControllers();

// add services
services.AddCache();
services.AddModelPolicies();
services.AddPersistence();
services.AddRepositoryServices();

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