using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Models.Domain
{
    public class State
    {
        public int Id { get; set; }
        public int Country_Id { get; set; }
        [ForeignKey("Country_Id")]
        public Countries? Country { get; set; }
        public string StateName { get; set; }
    }
}
