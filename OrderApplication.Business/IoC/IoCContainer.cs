using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Service.Concretion.Mongo;
using OrderApplication.Core.Business.Abstraction.RuleEngine;
using OrderApplication.Core.Business.Concretion.RuleEngine;
using OrderApplication.Core.Data.Abstraction.Mongo;
using OrderApplication.Core.Data.Concretion;
using OrderApplication.Core.Model.Util.AppSettings;

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
            #region Category

            //serviceCollection.AddScoped<IValidator<NewDashboardItemModel>, NewDashboardItemValidation>();
            //serviceCollection.AddScoped<IValidator<UpdateDashboardItemModel>, UpdateDashboardItemValidation>();

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
