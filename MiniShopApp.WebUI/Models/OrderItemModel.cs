namespace MiniShopApp.WebUI.Models
{
    public class OrderItemModel
    {
        public int OrtemItemId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

    }
}