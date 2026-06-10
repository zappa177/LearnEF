using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }

        //phuong thuc them moi person, tra ve personResponse , tao them country name cho personResponse
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
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
                _persons.Add(person);
                return ConvertPersonToPersonResponse(person);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the person.", ex);
            }


        }

        public List<PersonResponse> GetAllPersons()
        {
            try
            {
                if (_persons == null)
                {
                    throw new Exception("No persons found.");
                }
                if (_persons.Count == 0)
                {
                    return new List<PersonResponse>();
                }
                return _persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all persons.", ex);
            }
        }

        public PersonResponse GetPersonByPersonID(Guid? personID)
        {
            try
            {
                if (personID == null)
                {
                    throw new ArgumentNullException(nameof(personID));
                }
                Person? person = _persons.FirstOrDefault(p => p.PersonID == personID);
                if (person == null)
                {
                    throw new ArgumentException("Person not found.", nameof(personID));
                }
                return ConvertPersonToPersonResponse(person);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the person by ID.", ex);
            }

        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonID == personUpdateRequest.PersonID);
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
            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid personID)
        {
            if (personID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personID));
            }
            Person? personToDelete = _persons.FirstOrDefault(p => p.PersonID == personID);
            if (personToDelete == null)
                return false;
            _persons.RemoveAll(p => p.PersonID == personID);
            return true;

        }



        public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }
            return matchingPersons;
        }



        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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
