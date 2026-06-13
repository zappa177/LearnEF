using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
namespace Services
{
    public class PersonService : IPersonService
    {
        //private readonly PersonsDbContext _db;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;

        public PersonService(IPersonsRepository personsRepository, ICountriesService countriesService)
        {
            _personsRepository = personsRepository;
            _countriesService = countriesService;
        }

        //phuong thuc them moi person, tra ve personResponse , tao them country name cho personResponse

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            try
            {
                if (personAddRequest == null)
                {
                    throw new ArgumentNullException(nameof(personAddRequest));
                }
                if (string.IsNullOrEmpty(personAddRequest.PersonName))
                {
                    throw new ArgumentException("Person name cannot be null or empty.", nameof(personAddRequest.PersonName));
                }
                //validate personAddRequest using ValidationHelper
                ValidationHelper.ModelValidation(personAddRequest);
                //convert personAddRequest to person
                Person person = personAddRequest.ToPerson();
                person.PersonID = Guid.NewGuid();
                await _personsRepository.AddPerson(person);
                return person.ToPersonResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the person.", ex);
            }


        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personsRepository.GetAllPersons();


            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            try
            {
                if (personID == null)
                {
                    return null;
                }
                Person? person = await _personsRepository.GetAllPersonsByPersonID(personID.Value);
                if (person == null)
                {
                    return null;
                }
                return person.ToPersonResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the person by ID.", ex);
            }

        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = await _personsRepository.GetAllPersonsByPersonID(personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Person not found.", nameof(personUpdateRequest.PersonID));
            }
            //update all details

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender?.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiverNewsletter = personUpdateRequest.ReceiverNewsletter;
            //
            await _personsRepository.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid personID)
        {
            if (personID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personID));
            }
            Person? personToDelete = await _personsRepository.GetAllPersonsByPersonID(personID);
            if (personToDelete == null)
                return false;
            await _personsRepository.DeletePersonByPersonID(personID);

            return true;

        }



        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                // Nếu không có searchString thì trả về tất cả
                var allPersons = await _personsRepository.GetAllPersons();
                return allPersons.Select(p => p.ToPersonResponse()).ToList();
            }

            List<Person> persons = searchBy switch
            {
                nameof(PersonResponse.PersonName) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                        temp.PersonName != null &&
                        temp.PersonName.Contains(searchString)),

                nameof(PersonResponse.DateOfBirth) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                        temp.DateOfBirth.HasValue &&
                        temp.DateOfBirth.Value.ToString("yyyy-MM-dd")
                            .Contains(searchString)),

                nameof(PersonResponse.Gender) =>
                    await _personsRepository.GetFilteredPersons(temp =>
                        temp.Gender != null &&
                        temp.Gender.Equals(searchString)),

                _ => await _personsRepository.GetAllPersons()
            };

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }




        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }
            List<PersonResponse> sortedPersons = allPersons;
            switch (sortBy, sortOrder)
            {
                case (nameof(Person.PersonName), SortOrderOptions.ASC):
                    sortedPersons = allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                    break;
                case (nameof(Person.PersonName), SortOrderOptions.DSC):
                    sortedPersons = allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                    break;
            }
            return sortedPersons;
        }
    }
}
