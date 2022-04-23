namespace OrderApplication.Model.Document.Common.Customer
{
    public  class UpdateCustomerModel: OrderApplication.Model.Document.Customer
    {
        public UpdateCustomerModel()
        {
            this.UpdatedAt = DateTime.Now;
        }
    }
}
