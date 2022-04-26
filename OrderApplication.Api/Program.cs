using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OrderApplication.Api.MvcFilters;
using OrderApplication.Business.Autofac;
using OrderApplication.Business.Middleware;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Service.Concretion.Mongo;
using OrderApplication.Business.Validation.Customer;
using OrderApplication.Business.Validation.Order;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.RuleEngine;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Data.Concretion;
using OrderApplication.Core.Extension;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Model.Document;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});
// Add services to the container.

ConfigurationManager configuration = builder.Configuration;
var mongoSettingsSection = configuration.GetSection("MongoDbSettings");
builder.Services.Configure<MongoDbSettings>(mongoSettingsSection);

var appSettingsSection = configuration.GetSection("AppSetting");
builder.Services.Configure<AppSetting>(appSettingsSection);

builder.Services.AddControllers().AddMvcOptions(opt =>
{
    // opt.Filters.Add<ResourceFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

builder.Services.AddScoped<IBusinessRuleEngine, BusinessRuleEngine>();

builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

#region Validation

#region Customer

builder.Services.AddScoped<IValidator<Customer>, NewCustomerValidator>();
builder.Services.AddScoped<IValidator<Customer>, UpdateCustomerValidator>();

#endregion

#region Order

builder.Services.AddScoped<IValidator<Order>, NewOrderValidator>();
builder.Services.AddScoped<IValidator<Order>, UpdateOrderValidator>();

#endregion

#endregion

// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWelcomePage("/swagger");
app.UseAuthMiddleware<CustomMiddleware>();
app.UseExceptionMiddleware<CustomExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
