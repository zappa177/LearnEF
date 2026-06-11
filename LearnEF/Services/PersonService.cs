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

        public PersonService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
            if (initialize)
            {
                _persons.AddRange(new List<Person>()
                    {
                        new Person()
                        {
                            PersonID = Guid.Parse("C72F55DF-CC1A-468F-BD98-8BBB0E6E5942"),
                            PersonName = "Wadsworth",
                            Email = "wiskov0@devhub.com",
                            DateOfBirth = DateTime.Parse("1/17/2018"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "6 Rigney Road",
                            ReceiverNewsletter = true
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("7BD92340-2BDE-450C-A423-DC8C115E9D37"),
                            PersonName = "Gustavo",
                            Email = "gextall1@google.it",
                            DateOfBirth = DateTime.Parse("2/9/2005"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "4 Golf Course Center",
                            ReceiverNewsletter = false
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("2E56F780-8E16-4A97-9FFC-9DC1B24C394A"),
                            PersonName = "Gilberto",
                            Email = "gdoick2@tiny.cc",
                            DateOfBirth = DateTime.Parse("6/29/2001"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "2824 Fisk Trail",
                            ReceiverNewsletter = false
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("B1FCDB18-1EE7-4575-9E39-5AF643F134B6"),
                            PersonName = "Murial",
                            Email = "mreuven3@twitter.com",
                            DateOfBirth = DateTime.Parse("11/26/2009"),
                            Gender = "Female",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "51 Evergreen Plaza",
                            ReceiverNewsletter = true
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("77D07402-5542-4471-95F8-024CE7CE24A3"),
                            PersonName = "Dorie",
                            Email = "dmeredyth4@umn.edu",
                            DateOfBirth = DateTime.Parse("3/18/2023"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "392 Declaration Road",
                            ReceiverNewsletter = true
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("CCF647F8-CBAC-47DE-A345-6350D4F728AD"),
                            PersonName = "Nike",
                            Email = "nhasty5@github.com",
                            DateOfBirth = DateTime.Parse("8/25/2003"),
                            Gender = "Female",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "7071 Oriole Crossing",
                            ReceiverNewsletter = false
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("A826188D-793D-46F4-AA4C-B4B405FFB9E9"),
                            PersonName = "Juliette",
                            Email = "jwhittock6@state.tx.us",
                            DateOfBirth = DateTime.Parse("11/1/2002"),
                            Gender = "Female",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "4786 Sutherland Center",
                            ReceiverNewsletter = true
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("0C3E222E-137D-4DC2-B417-B6C5C956DA89"),
                            PersonName = "Pietro",
                            Email = "pportwaine7@rakuten.co.jp",
                            DateOfBirth = DateTime.Parse("11/27/2002"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "9 Stuart Lane",
                            ReceiverNewsletter = false
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("1624C585-78F2-43F6-8910-2DF2823969CE"),
                            PersonName = "Rorie",
                            Email = "rlude8@weather.com",
                            DateOfBirth = DateTime.Parse("1/29/2020"),
                            Gender = "Female",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "19335 Goodland Plaza",
                            ReceiverNewsletter = false
                        },
                        new Person()
                        {
                            PersonID = Guid.Parse("4677BB05-EE46-4AB0-B378-22B34101D759"),
                            PersonName = "Mordecai",
                            Email = "mmorch9@shop-pro.jp",
                            DateOfBirth = DateTime.Parse("1/11/2007"),
                            Gender = "Male",
                            CountryID = Guid.Parse("82588A06-588C-474B-8ACF-91BEFBA6E4D4"),
                            Address = "7084 Mcguire Road",
                            ReceiverNewsletter = false
                        }
                });
            }
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



        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }
            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => (!string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("yyyy-MM-dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(PersonResponse.Gender):
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
