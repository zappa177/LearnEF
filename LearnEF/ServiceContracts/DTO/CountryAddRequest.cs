using Entities;


namespace ServiceContracts.DTO
{
    //DTO cho country nhan du lieu tu client gui len de them moi country
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }


        public Country ToCountry()
        {
            return new Country { CountryName = CountryName };
        }
    }
}
