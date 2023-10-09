namespace Project1.Models
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }

        public int Role_Id { get; set; }

        public int Country_Id { get; set; }

        public int State_Id { get; set; }

        public int City_Id { get; set; }
    }

    public class UpdateUserViewModel : UserViewModel
    {
        public int Id { get; set; }
    }
}
