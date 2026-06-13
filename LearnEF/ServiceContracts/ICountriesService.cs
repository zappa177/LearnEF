using ServiceContracts.DTO;


namespace ServiceContracts
{
    //interface for country service chua dinh nghia cac phuong thuc lien quan den country
    public interface ICountriesService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        Task<List<CountryResponse>> GetAllCountries();
        Task<CountryResponse?> GetCountryByID(Guid countryID);
        //Task<CountryResponse?> UpdateCountry(CountryUpdateRequest? countryUpdateRequest);
        //Task<bool> DeleteCountry(Guid? countryID);
    }
}
