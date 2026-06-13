using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {

        public PersonsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        //

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");
            //seed data
            modelBuilder.Entity<Country>().HasData(
                new Country { CountryID = Guid.Parse("32DC48AF-B4A3-4304-9257-4B865BD6E2D0"), CountryName = "USA" }
                );
            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    PersonID = Guid.Parse("C72F55DF-CC1A-468F-BD98-8BBB0E6E5942"),
                    PersonName = "Wadsworth",
                    Email = "wiskov0@devhub.com",
                    DateOfBirth = DateTime.Parse("1/17/2018"),
                    Gender = "Male",
                    CountryID = Guid.Parse("32DC48AF-B4A3-4304-9257-4B865BD6E2D0"),
                    Address = "6 Rigney Road",
                    ReceiverNewsletter = true
                }
             );

            modelBuilder.Entity<Person>().Property(p => p.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("nvarchar(50)")
                .HasDefaultValue("Not Provided");
            //modelBuilder.Entity<Person>().HasIndex(p => p.TIN).IsUnique();
            modelBuilder.Entity<Person>().ToTable(p => p.HasCheckConstraint("CK_Person_TIN", "LEN(TaxIdentificationNumber) = 10 OR TaxIdentificationNumber = 'Not Provided'"));

            modelBuilder.Entity<Person>()
                .HasOne<Country>(c => c.Country)
                .WithMany(p => p.Persons)
                .HasForeignKey(p => p.CountryID);
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE ap_GetAllPersons").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiverNewsletter", person.ReceiverNewsletter)
            };
            return Database.ExecuteSqlRaw("EXECUTE sp_InsertPerson @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiverNewsletter", sp);
        }

    }
}
