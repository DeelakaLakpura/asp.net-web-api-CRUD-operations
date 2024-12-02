using System.ComponentModel.DataAnnotations;

namespace webapp.Model
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        public string? Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Postal_Code { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }
    }


}
