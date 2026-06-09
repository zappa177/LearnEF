using Entities;

namespace ServiceContracts.DTO
{
    //DTO cho country nhan du lieu tu service tra ve client
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse { CountryName = country.CountryName, CountryID = country.CountryID };
        }
    }
}
