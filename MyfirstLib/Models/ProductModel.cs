namespace MyfirstLib.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string UnitOfMeasure { get; set; } = null!;
    }
}
