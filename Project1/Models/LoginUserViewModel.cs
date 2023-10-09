using Microsoft.AspNetCore.Mvc.Rendering;
using Project1.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models
{
    public class LoginUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
