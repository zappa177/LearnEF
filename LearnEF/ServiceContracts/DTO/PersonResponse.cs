using Entities;
using ServiceContracts.Enums;
namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiverNewsletter { get; set; }
        public double? Age { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse other = (PersonResponse)obj;
            return this.PersonID == other.PersonID && this.PersonName == other.PersonName && this.Email == other.Email
                && this.DateOfBirth == other.DateOfBirth && this.Gender == other.Gender && this.CountryID == other.CountryID
                && this.Address == other.Address && this.ReceiverNewsletter == other.ReceiverNewsletter;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiverNewsletter);
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonID = this.PersonID,
                PersonName = this.PersonName,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth,
                Gender = (GenderOption)Enum.Parse(typeof(GenderOption), this.Gender ?? nameof(GenderOption.Other), true),
                CountryID = this.CountryID ?? Guid.Empty,
                Address = this.Address,
                ReceiverNewsletter = this.ReceiverNewsletter
            };
        }
    }

    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Email = person.Email,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiverNewsletter = person.ReceiverNewsletter,
                Age = person.DateOfBirth.HasValue ? (int)((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : (int?)null,
                Country = person.Country?.CountryName
            };
        }
    }
}
