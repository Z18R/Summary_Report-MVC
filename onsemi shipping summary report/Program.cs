using LotTrackingApp.Services;
using Microsoft.EntityFrameworkCore;
using onsemi_shipping_summary_report.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ProductionOrderService>();
builder.Services.AddScoped<WaferService>();
builder.Services.AddScoped<SearchGemAssyService>();
builder.Services.AddScoped<DeviceService>();
builder.Services.AddScoped<ExportService>();
builder.Services.AddScoped<LotTrackingService>();
builder.Services.AddScoped<DatalogsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();

