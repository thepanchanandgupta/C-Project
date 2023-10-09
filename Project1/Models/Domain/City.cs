using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models.Domain
{
    public class City
    {
        public int Id { get; set; }
        public int? State_Id { get; set; }
        [ForeignKey("State_Id")]
        public State? State { get; set; }
        public int? Country_Id { get; set; }
        [ForeignKey("Country_Id")]
        public Countries? Country { get; set; }
        public string CityName { get; set; }
    }
}