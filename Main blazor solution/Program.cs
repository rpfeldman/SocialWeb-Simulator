using LoginWeb.Components;
using LoginWeb.InnerComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer;
using USModel;
using MSModel;
using MSRepositories;
using MSServices;
using USRepositories;
using USServices;
using System.Reflection.Metadata.Ecma335;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Get values configurated by appsettings.json
var ConnectionString = builder.Configuration.GetSection("DataBase").GetValue<string>("ConnectionString");

int MessageTextCharLimits = builder.Configuration.GetSection("DataBase").GetValue<int>("MessageTextCharLimit");
int[] UsernameCharLimits = { builder.Configuration.GetSection("DataBase").GetValue<int>("UsernameMinCharLimit"), builder.Configuration.GetSection("DataBase").GetValue<int>("UsernameMaxCharLimit") };
int[] PasswordCharLimits = { builder.Configuration.GetSection("DataBase").GetValue<int>("PasswordMinCharLimit"), builder.Configuration.GetSection("DataBase").GetValue<int>("PasswordMaxCharLimit") };

var Implementation = builder.Configuration.GetSection("DataBase").GetValue<UserDataBaseImplementation>("Implementation");

// Sets and configurates EF core DB Context for user system database
var UserDBContextOptions = new DbContextOptionsBuilder<UserDbContext>().UseSqlServer(ConnectionString).Options;
var UserDBContext = new UserDbContext(UserDBContextOptions, UsernameCharLimits, PasswordCharLimits);

// Sets and configurates EF core DB context for message system database
var MessageContainerContextOptions = new DbContextOptionsBuilder<MessageContainerDbContext>().UseSqlServer(ConnectionString).Options;
var MessageContainerContext = new MessageContainerDbContext(MessageContainerContextOptions, MessageTextCharLimits);

// Sets and configurates inner components services as URL security and global property DTO
builder.Services.AddScoped<LoginSecurityService>(); // URL and login security service
builder.Services.AddSingleton<GlobalPropertysService>(sp => { return new GlobalPropertysService(ConnectionString!, UsernameCharLimits, PasswordCharLimits); }); // Service to share the properties from appsettings.json

// Dependencies for the registration, login and user-management services
switch (Implementation)
{
    case UserDataBaseImplementation.Test:

        builder.Services.AddSingleton<IUserDbRepo, Test_UserRepository>();
        // Adds to blazor DI message container repository
        builder.Services.AddSingleton<IMessageContainer<Message, int>, Test_MessageContainer>();
        break;

    case UserDataBaseImplementation.Dapper:

        builder.Services.AddScoped<IUserDbRepo, Dapper_UserRepository>(sp => { return new Dapper_UserRepository(ConnectionString!, "Users", UsernameCharLimits, PasswordCharLimits); });
        // Adds to blazor DI message container repository
        builder.Services.AddScoped<IMessageContainer<Message, int>, EF_MessageContainer>(sp => { return new EF_MessageContainer(MessageContainerContext); });

        break;

    case UserDataBaseImplementation.Entity_framework:

        builder.Services.AddSingleton<IUserDbRepo, EntityFramework_UserRepository>(sp => { return new EntityFramework_UserRepository(UserDBContext); });
        // Adds to blazor DI message container repository
        builder.Services.AddScoped<IMessageContainer<Message, int>, EF_MessageContainer>(sp => { return new EF_MessageContainer(MessageContainerContext); });

        break;

    default:
        break; 
}


// Services that handle the user system: registration, authentication and projection
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<UserRegistrationService>();
builder.Services.AddScoped<ViewProjectionService>();

// Services that handle the messaging system
builder.Services.AddScoped<MessageSenderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
