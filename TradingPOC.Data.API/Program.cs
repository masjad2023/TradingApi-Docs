using Microsoft.AspNetCore.Authentication.Cookies;
using TradingPOC.Data.API.Models.Settings;
using TradingPOC.Data.API.Service;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region DI
var config = new ConfigurationBuilder()
                         .AddJsonFile($"appsettings.json", optional: false)
                         .Build();

builder.Services.AddCors(p => p.AddPolicy(name: MyAllowSpecificOrigins, builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
var appSettings = config.GetSection("AppSettings").Get<AppSettings>();

builder.Services.AddSingleton<RapidAPISettings>(appSettings.RapidAPISettings);
builder.Services.AddTransient<ILivePriceService, RapidAPILivePriceService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
