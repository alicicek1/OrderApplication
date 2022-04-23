namespace OrderApplication.Model.Document.Common.Customer
{
    public class NewCustomerModel: OrderApplication.Model.Document.Customer
    {
        public NewCustomerModel()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
