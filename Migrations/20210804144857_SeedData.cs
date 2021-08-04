using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.API.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthdata",
                table: "Authors",
                newName: "BirthDate");

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDate", "BirthPlace", "Email", "Name" },
                values: new object[] { new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"), new DateTimeOffset(new DateTime(1996, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), "AAAA", "author1@xxx.com", "Author 1" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Description", "Pages", "Title" },
                values: new object[] { new Guid("3dadbbe9-3a75-434e-a9f9-9cc924f5d212"), new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"), null, 200, "Book 1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("3dadbbe9-3a75-434e-a9f9-9cc924f5d212"));

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: new Guid("72d5b5f5-3008-49b7-b0d6-cc337f1a3330"));

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Authors",
                newName: "Birthdata");
        }
    }
}
