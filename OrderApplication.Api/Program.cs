using OrderApplication.Business.IoC;
using OrderApplication.Core.Model.Util.AppSettings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
var mongoSettingsSection = configuration.GetSection("MongoDbSettings");
builder.Services.Configure<MongoDbSettings>(mongoSettingsSection);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Load();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderApp v1");
    });
}

app.UseWelcomePage("/swagger");
app.UseAuthorization();

app.MapControllers();

app.Run();
