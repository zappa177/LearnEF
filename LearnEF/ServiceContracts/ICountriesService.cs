using ServiceContracts.DTO;


namespace ServiceContracts
{
    //interface for country service chua dinh nghia cac phuong thuc lien quan den country
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        //List<CountryResponse> GetAllCountries();
        //CountryResponse? GetCountryByID(Guid countryID);
        //CountryResponse? UpdateCountry(CountryUpdateRequest? countryUpdateRequest);
        //bool DeleteCountry(Guid countryID);
    }
}
