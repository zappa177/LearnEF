using System.ComponentModel.DataAnnotations;

namespace Entities
{
    //domain model for country
    public class Country
    {
        [Key]
        public Guid CountryID { get; set; }
        [StringLength(40)]
        public string? CountryName { get; set; }

        public virtual ICollection<Person>? Persons { get; set; }
    }
}
