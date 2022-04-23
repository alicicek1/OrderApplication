namespace OrderApplication.Model.Document.Common.Order
{
    public class UpdateOrderModel : OrderApplication.Model.Document.Order
    {
        public UpdateOrderModel()
        {
            this.UpdatedAt = DateTime.Now;
        }
    }
}
