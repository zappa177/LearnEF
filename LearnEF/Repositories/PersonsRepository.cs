using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System.Linq.Expressions;

namespace Repositories
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly PersonsDbContext _db;
        private readonly ILogger<PersonsRepository> _logger;
        public PersonsRepository(PersonsDbContext personsDbContext , ILogger<PersonsRepository> logger)
        {
            _db = personsDbContext;
            _logger = logger; 
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(p => p.PersonID == personID));
            int rowDeleted = await _db.SaveChangesAsync();
            return rowDeleted > 0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            //return await _db.Persons.Include("Country").ToListAsync();
            return await _db.Persons.Include("Country").ToListAsync();
        }


        public async Task<Person?> GetAllPersonsByPersonID(Guid? personID)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(p => p.PersonID == personID);
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            _logger.LogInformation("get filtered person of person repository");
            return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(p => p.PersonID == person.PersonID);
            if (matchingPerson == null)
            {
                return person;
            }
            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.Address = person.Address;
            matchingPerson.ReceiverNewsletter = person.ReceiverNewsletter;
            int countUpdated = await _db.SaveChangesAsync();
            return matchingPerson;
        }


    }
}
