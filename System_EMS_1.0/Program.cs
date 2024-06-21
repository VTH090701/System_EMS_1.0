using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using Radzen;
using System.Globalization;
using System_EMS_1._0.Data;
using System_EMS_1._0.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddHttpClient();



//builder.Services.AddScoped<Api>();
builder.Services.AddScoped<IApi, Api>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddTransient<IDeviceService, DeviceService>();


//Config
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

//Set datetime VietNam
var supportedCultures = new[] { new CultureInfo("vi-VN") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);
//




app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
