using Entities;

namespace ServiceContracts.DTO
{
    //DTO cho country nhan du lieu tu service tra ve client
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }
        //override equals method de so sanh 2 countryResponse co cung countryID va countryName hay khong
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(CountryResponse)) return false;
            CountryResponse other = (CountryResponse)obj;
            return this.CountryID == other.CountryID && this.CountryName == other.CountryName;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(CountryID, CountryName);
        }

        //
        public CountryUpdateRequest ToCountryUpdateRequest()
        {
            return new CountryUpdateRequest
            {
                CountryID = this.CountryID,
                CountryName = this.CountryName
            };
        }
    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse { CountryName = country.CountryName, CountryID = country.CountryID };
        }
    }
}
