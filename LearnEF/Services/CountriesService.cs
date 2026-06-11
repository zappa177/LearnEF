using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.AddRange(new List<Country>()
                {
                        new Country()
                    {
                        CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                        CountryName = "India"
                    },
                    new Country()
                    {
                        CountryID = Guid.Parse("04FB660F-5C7A-455D-8E7A-5D8D8D69DEC5"),
                        CountryName = "USA"
                    },
                    new Country()
                    {
                        CountryID = Guid.Parse("45D1B057-03F5-43AC-84B1-0EEE37251D1F"),
                        CountryName = "Canada"
                    },
                    new Country()
                    {
                        CountryID = Guid.Parse("45C572DC-8B1E-4295-8988-5B3A20140F13"),
                        CountryName = "UK"
                    },
                    new Country()
                    {
                        CountryID = Guid.Parse("70BD061F-C932-4FFC-952D-2A45A070CC3D"),
                        CountryName = "Australia"
                    }
                });
            }
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            try
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
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while adding a country: {ex.Message}");
                // Rethrow the exception or return a default response based on your application's needs
                throw;

            }

        }

        public List<CountryResponse> GetAllCountries()
        {
            try
            {
                return _countries?.Select(country => country.ToCountryResponse()).ToList() ?? new List<CountryResponse>();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving countries: {ex.Message}");
                // Return an empty list or rethrow the exception based on your application's needs
                return new List<CountryResponse>();
            }
        }

        public CountryResponse? GetCountryByID(Guid countryID)
        {
            try
            {
                //countryID khong duoc empty
                if (countryID == Guid.Empty)
                {
                    throw new ArgumentException("Country ID cannot be empty.", nameof(countryID));
                }
                //tim country trong list bang countryID
                Country? country = _countries.FirstOrDefault(temp => temp.CountryID == countryID);
                if (country == null)
                {
                    throw new KeyNotFoundException($"Country with ID '{countryID}' not found.");
                }
                return country?.ToCountryResponse();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving the country by ID: {ex.Message}");
                // Return null or rethrow the exception based on your application's needs
                return null;
            }
        }

        public CountryResponse? UpdateCountry(CountryUpdateRequest? countryUpdateRequest)
        {
            if (countryUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(countryUpdateRequest));
            }
            //validation
            ValidationHelper.ModelValidation(countryUpdateRequest);
            Country? country = _countries.FirstOrDefault(temp => temp.CountryID == countryUpdateRequest.CountryID);
            if (country == null)
            {
                throw new KeyNotFoundException("Country with the specified ID not found.");
            }
            country.CountryName = countryUpdateRequest.CountryName;
            return country.ToCountryResponse();

        }

        public bool DeleteCountry(Guid? countryID)
        {
            if (countryID == null)
            {
                throw new ArgumentNullException(nameof(countryID));
            }
            Country? country = _countries.FirstOrDefault(temp => temp.CountryID == countryID);
            if (country == null)
            {
                throw new KeyNotFoundException("Country with the specified ID not found.");
            }
            _countries.RemoveAll(temp => temp.CountryID == countryID);
            return true;
        }
    }
}
