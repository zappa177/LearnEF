using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //kiemtra dto null hay khong, ten quoc gia null hoac empty, ten quoc gia da ton tai
            if (countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            //kiem tra ten quoc gia null hoac empty
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }
            //kiem tra ten quoc gia da ton tai
            if (_countries.Any(temp => temp.CountryName == countryAddRequest.CountryName))
            {
                throw new InvalidOperationException($"Country with name '{countryAddRequest.CountryName}' already exists.");
            }

            //chuyen doi tu countryAddRequest sang country
            Country country = countryAddRequest.ToCountry();
            //tao guid moi cho country
            country.CountryID = Guid.NewGuid();
            //luu country vao list
            _countries.Add(country);
            //chuyen doi tu country sang countryResponse
            return country.ToCountryResponse();
        }
    }
}
