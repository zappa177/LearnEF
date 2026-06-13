using ServiceContracts.DTO;
using ServiceContracts.Enums;



namespace ServiceContracts
{
    public interface IPersonService
    {
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
        Task<List<PersonResponse>> GetAllPersons();
        //PersonResponse AddPerson(string personName);
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        Task<bool> DeletePerson(Guid personID);

        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
    }
}
