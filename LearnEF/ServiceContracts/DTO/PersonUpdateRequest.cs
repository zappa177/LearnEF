using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "Person ID is required.")]
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Person name is required.")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOption? Gender { get; set; }
        [Required(ErrorMessage = "Country ID is required.")]
        public Guid CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiverNewsletter { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                PersonID = this.PersonID,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender.ToString(),
                CountryID = this.CountryID,
                Address = this.Address,
                ReceiverNewsletter = this.ReceiverNewsletter
            };
        }
    }
}
