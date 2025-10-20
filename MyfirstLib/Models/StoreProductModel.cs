namespace MyfirstLib.Models
{
    public class StoreProductModel
    {
        public int StoreProductId { get; set; }

        public int StoreId { get; set; }
        public string Store { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
    }
}
