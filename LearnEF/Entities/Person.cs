using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [StringLength(40)]
        public string? PersonName { get; set; }
        [StringLength(40)]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }
        //unique identifier for country, foreign key to country table
        public Guid CountryID { get; set; }
        [StringLength(100)]
        public string? Address { get; set; }
        //bit
        public bool ReceiverNewsletter { get; set; }

        public string? TIN { get; set; }

        public virtual Country? Country { get; set; }
    }
}
