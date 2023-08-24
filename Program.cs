using AspNet.Security.OAuth.Validation;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(OAuthValidationDefaults.AuthenticationScheme).AddOAuthValidation();
builder.Services.AddHttpClient();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method