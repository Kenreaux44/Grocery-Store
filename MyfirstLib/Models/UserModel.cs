namespace MyfirstLib.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime CreatedDate { get; set; }  // Do not include and display in UI

        public string CreatedBy { get; set; } = null!; // Do not include and display in UI

        public DateTime? LastUpdateDate { get; set; }  // Do not include and display in UI

        public string? UpdatedBy { get; set; }  // Do not include and display in UI
    }
}
