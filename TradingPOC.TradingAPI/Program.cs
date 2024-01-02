using TradingPOC.TradingAPI.Entities;
using TradingPOC.TradingAPI.Service;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TradingPOC.TradingAPI.ServiceBusExtention;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfigurationRoot configuration = new ConfigurationBuilder()
                         .AddJsonFile($"appsettings.json", optional: false)
                         .Build();

builder.Services.AddCors(p => p.AddPolicy(name: MyAllowSpecificOrigins, builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContext<TradingDbContext>(options =>
{
    options.UseMySQL(configuration.GetConnectionString("TradingDbContext"));
}); 

builder.Services.AddSingleton(sp =>
{
	var rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

	// Set up RabbitMQ connection factory
	var factory = new ConnectionFactory {
		HostName = rabbitMQConfig?.HostName,
		Port = 5672,
		UserName = rabbitMQConfig?.UserId,
		Password = rabbitMQConfig?.Password
	};

	return new RabbitMQPublisher(factory);
});

builder.Services.AddTransient<ITradingService, TradingService>();
builder.Services.AddTransient<IReportService, ReportService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

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
