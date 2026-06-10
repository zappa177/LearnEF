using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class CountryUpdateRequest
    {
        [Required(ErrorMessage = "Country ID is required.")]
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }


        public Country ToCountry()
        {
            return new Country { CountryName = CountryName };
        }
    }
}
