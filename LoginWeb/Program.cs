using LoginWeb.Components;
using LoginWeb.InnerServices;
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

var ConnectionString = builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("ConnectionString");

var UserDBContextOptions = new DbContextOptionsBuilder<UserDbContext>().UseSqlServer(ConnectionString).Options;
var UserDBContext = new UserDbContext(UserDBContextOptions, new int[] { 5, 30 }, new int[] { 5, 60 });

var MessageContainerContextOptions = new DbContextOptionsBuilder<MessageContainerDbContext>().UseSqlServer(ConnectionString).Options;
var MessageContainerContext = new MessageContainerDbContext(MessageContainerContextOptions, 150);


builder.Services.AddSingleton<LoginSecurityService>(); // Servicio de seguridad de URL y logueo

// Servicios que manejan el registro, inicio de sesion y manejo de usuarios
builder.Services.AddSingleton<IUserDbRepo, EntityFramework_UserRepository>(sp => { return new EntityFramework_UserRepository(UserDBContext); });
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<UserRegistrationService>();
builder.Services.AddScoped<ViewProjectionService>();

// Servicios que manejan el sistema de mensajeria
builder.Services.AddScoped<IMessageContainer<Message, int>, EF_MessageContainer>(sp => { return new EF_MessageContainer(MessageContainerContext); });
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
