using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public partial class SystemAccountDTO
    {
        [Key]
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account Name is required.")]
        [StringLength(100, ErrorMessage = "Account Name cannot be longer than 100 characters.")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string AccountPassword { get; set; }
    }
}
