using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Project1.Models.Domain
{
    [Index(nameof(Email), IsUnique =true)]
    [Index(nameof(PhoneNumber), IsUnique =true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }

        public string Password { get; set; }
        public int Role_Id { get; set; }
        [ForeignKey("Role_Id")]
        public Role? Role { get; set; }

        public int? Country_Id { get; set; }
        [ForeignKey("Country_Id")]
        public Countries? Country { get; set; }

        public int? State_Id { get; set; }
        [ForeignKey("State_Id")]
        public State? State { get; set; }
        public int? City_Id { get; set; }
        [ForeignKey("City_Id")]
        public City? City { get; set; }



    }
}