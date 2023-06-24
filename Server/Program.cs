using GMBL.Server;
using GMBL.Server.Interfaces;
using GMBL.Server.Services;
using GMBL.Server.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<AppSettings>(builder.Configuration);


builder.Services.AddScoped<ISteamAuthService, SteamAuthService>();
builder.Services.AddScoped<ISteamInventoryService, SteamInventoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<SteamSessionManager>();


builder.Services.AddSession(options =>
{
    options.Cookie.Name = "TimeLogin";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Sitzungsdauer in Minuten anpassen
                                                    // Weitere Sitzungsoptionen konfigurieren, falls erforderlich
});


ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.ClientId = "592DCC71662AD677CFB9A491FE5A9F18";
    options.Authority = "https://steamcommunity.com/openid/";
    options.CallbackPath = "/signin-steam";
    options.ResponseType = "id_token";
    options.SaveTokens = true;
    options.TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = false };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
            builder =>
            {
                builder.WithOrigins("https://localhost:44357")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    IdentityModelEventSource.ShowPII = true;
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseBlazorFrameworkFiles();
app.UseCors("AllowLocalhost");
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
