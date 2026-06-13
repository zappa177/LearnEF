using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"CREATE PROCEDURE sp_InsertPerson
                                (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(40), @DateOfBirth date, @Gender nvarchar(10), @CountryID uniqueidentifier, @Address nvarchar(100), @ReceiverNewsletter bit)
                                AS
                                BEGIN
                                    INSERT INTO Persons (PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiverNewsletter)
                                    VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiverNewsletter)
                                END";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_InsertPerson");

        }
    }
}
