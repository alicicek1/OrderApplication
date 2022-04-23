namespace OrderApplication.Model.Document.Common.Order
{
    public class NewOrderModel : OrderApplication.Model.Document.Order
    {
        public NewOrderModel()
        {
            this.CreatedAt = DateTime.Now;
        }
    }
}
