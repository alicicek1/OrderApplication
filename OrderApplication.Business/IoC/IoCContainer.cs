using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Service.Concretion.Mongo;
using OrderApplication.Business.Validation.Customer;
using OrderApplication.Business.Validation.Order;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.RuleEngine;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Data.Concretion;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Model.Document;

namespace OrderApplication.Business.IoC
{
    public static class IoCContainer
    {
        public static void AddRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        }

        public static void AddSingleton(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMongoDbSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        }

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IOrderService, OrderService>();
            serviceCollection.AddScoped<ICustomerService, CustomerService>();
        }

        public static void AddValidation(this IServiceCollection serviceCollection)
        {
            #region Customer

            serviceCollection.AddScoped<IValidator<Customer>, NewCustomerValidator>();
            serviceCollection.AddScoped<IValidator<Customer>, UpdateCustomerValidator>();

            #endregion

            #region Order

            serviceCollection.AddScoped<IValidator<Order>, NewOrderValidator>();
            serviceCollection.AddScoped<IValidator<Order>, UpdateOrderValidator>();

            #endregion
        }
        public static void AddOthersUtils(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IBusinessRuleEngine, BusinessRuleEngine>();
        }

        public static void Load(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton();
            serviceCollection.AddRepository();
            serviceCollection.AddServices();
            serviceCollection.AddValidation();
            serviceCollection.AddOthersUtils();
        }
    }
}
