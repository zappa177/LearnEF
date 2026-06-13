using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private readonly PersonsDbContext _db;
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {

            //kiemtra dto null hay khong, ten quoc gia null hoac empty, ten quoc gia da ton tai
            if (countryAddRequest == null) { throw new ArgumentNullException(nameof(countryAddRequest)); }
            //kiem tra ten quoc gia null hoac empty
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException("Country name cannot be null or empty.", nameof(countryAddRequest.CountryName));
            }
            //kiem tra ten quoc gia da ton tai
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new InvalidOperationException($"Country with name '{countryAddRequest.CountryName}' already exists.");
            }

            //chuyen doi tu countryAddRequest sang country
            Country country = countryAddRequest.ToCountry();
            //tao guid moi cho country
            country.CountryID = Guid.NewGuid();
            //luu country vao list
            await _countriesRepository.AddCountry(country);
            //chuyen doi tu country sang countryResponse
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();
        }


        //public async Task<CountryResponse?> UpdateCountry(CountryUpdateRequest? countryUpdateRequest)
        //{
        //    if (countryUpdateRequest == null)
        //    {
        //        throw new ArgumentNullException(nameof(countryUpdateRequest));
        //    }
        //    //validation
        //    ValidationHelper.ModelValidation(countryUpdateRequest);
        //    Country? country = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryUpdateRequest.CountryID);
        //    if (country == null)
        //    {
        //        throw new KeyNotFoundException("Country with the specified ID not found.");
        //    }
        //    country.CountryName = countryUpdateRequest.CountryName;
        //    await _db.SaveChangesAsync();
        //    return country.ToCountryResponse();

        //}

        //public async Task<bool> DeleteCountry(Guid? countryID)
        //{
        //    if (countryID == null)
        //    {
        //        throw new ArgumentNullException(nameof(countryID));
        //    }
        //    Country? country = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryID == countryID);
        //    if (country == null)
        //    {
        //        throw new KeyNotFoundException("Country with the specified ID not found.");
        //    }
        //    _db.Countries.Remove(country);
        //    await _db.SaveChangesAsync();
        //    return true;
        //}

        public async Task<CountryResponse?> GetCountryByID(Guid countryID)
        {
            //countryID khong duoc empty
            if (countryID == Guid.Empty)
            {
                return null;
            }
            //tim country trong list bang countryID
            Country? country = await _countriesRepository.GetCountryByCountryID(countryID);
            if (country == null)
            {
                return null;
            }
            return country.ToCountryResponse();
        }
    }
}
