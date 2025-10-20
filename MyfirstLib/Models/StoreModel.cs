namespace MyfirstLib.Models
{
    public class StoreModel
    {
        public int StoreId { get; set; }

        public string Name { get; set; } = null!;

        public string Address1 { get; set; } = null!;

        public string? Address2 { get; set; }

        public string City { get; set; } = null!;

        public int StateId { get; set; }
        public string State { get; set; }

        public string PostalCode { get; set; } = null!;

        public DateTime CreatedDate { get; set; }  // Do not include and display in UI

        public string CreatedBy { get; set; } = null!;  // Do not include and display in UI

        public DateTime? LastUpdateDate { get; set; }  // Do not include and display in UI

        public string? UpdatedBy { get; set; }  // Do not include and display in UI
    }
}
