﻿using OrderApplication.Core.Business.Abstraction.Mongo;
using OrderApplication.Model.Document;

namespace OrderApplication.Business.Service.Abstraction.Mongo
{
    public interface ICustomerService : IMongoService<Customer>
    {
    }
}
