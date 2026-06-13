using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string ap_GetAllPersons = @"CREATE PROCEDURE ap_GetAllPersons
                                AS
                                BEGIN
                                    SELECT * FROM Persons
                                END";
            migrationBuilder.Sql(ap_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE ap_GetAllPersons");

        }
    }
}
