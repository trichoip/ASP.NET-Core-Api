using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AspNetCore.CleanArchitecture.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Countries",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "longtext", nullable: false),
                TwoLetterIsoCode = table.Column<string>(type: "longtext", nullable: false),
                ThreeLetterIsoCode = table.Column<string>(type: "longtext", nullable: false),
                FlagUrl = table.Column<string>(type: "longtext", nullable: false),
                DisplayOrder = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                ModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                ModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Countries", x => x.Id);
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Stadiums",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "longtext", nullable: false),
                PhotoUrl = table.Column<string>(type: "longtext", nullable: false),
                Capacity = table.Column<int>(type: "int", nullable: true),
                BuiltYear = table.Column<int>(type: "int", nullable: true),
                PitchLength = table.Column<int>(type: "int", nullable: true),
                PitchWidth = table.Column<int>(type: "int", nullable: true),
                Phone = table.Column<string>(type: "longtext", nullable: false),
                AddressLine1 = table.Column<string>(type: "longtext", nullable: false),
                AddressLine2 = table.Column<string>(type: "longtext", nullable: false),
                AddressLine3 = table.Column<string>(type: "longtext", nullable: false),
                City = table.Column<string>(type: "longtext", nullable: false),
                PostalCode = table.Column<string>(type: "longtext", nullable: false),
                CountryId = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                ModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                ModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Stadiums", x => x.Id);
                table.ForeignKey(
                    name: "FK_Stadiums_Countries_CountryId",
                    column: x => x.CountryId,
                    principalTable: "Countries",
                    principalColumn: "Id");
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Clubs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "longtext", nullable: false),
                PhotoUrl = table.Column<string>(type: "longtext", nullable: false),
                WebsiteUrl = table.Column<string>(type: "longtext", nullable: false),
                FacebookUrl = table.Column<string>(type: "longtext", nullable: false),
                TwitterUrl = table.Column<string>(type: "longtext", nullable: false),
                YoutubeUrl = table.Column<string>(type: "longtext", nullable: false),
                InstagramUrl = table.Column<string>(type: "longtext", nullable: false),
                StadiumId = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                ModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                ModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clubs", x => x.Id);
                table.ForeignKey(
                    name: "FK_Clubs_Stadiums_StadiumId",
                    column: x => x.StadiumId,
                    principalTable: "Stadiums",
                    principalColumn: "Id");
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "Players",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                ShirtNo = table.Column<int>(type: "int", nullable: true),
                ClubId = table.Column<int>(type: "int", nullable: true),
                PlayerPositionId = table.Column<int>(type: "int", nullable: true),
                PhotoUrl = table.Column<string>(type: "longtext", nullable: true),
                CountryId = table.Column<int>(type: "int", nullable: true),
                BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                HeightInCm = table.Column<int>(type: "int", nullable: true),
                FacebookUrl = table.Column<string>(type: "longtext", nullable: true),
                TwitterUrl = table.Column<string>(type: "longtext", nullable: true),
                InstagramUrl = table.Column<string>(type: "longtext", nullable: true),
                DisplayOrder = table.Column<int>(type: "int", nullable: true),
                CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                ModifiedBy = table.Column<string>(type: "longtext", nullable: true),
                ModifiedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Players", x => x.Id);
                table.ForeignKey(
                    name: "FK_Players_Clubs_ClubId",
                    column: x => x.ClubId,
                    principalTable: "Clubs",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Players_Countries_CountryId",
                    column: x => x.CountryId,
                    principalTable: "Countries",
                    principalColumn: "Id");
            })
            .Annotation("MySQL:Charset", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_Clubs_StadiumId",
            table: "Clubs",
            column: "StadiumId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_ClubId",
            table: "Players",
            column: "ClubId");

        migrationBuilder.CreateIndex(
            name: "IX_Players_CountryId",
            table: "Players",
            column: "CountryId");

        migrationBuilder.CreateIndex(
            name: "IX_Stadiums_CountryId",
            table: "Stadiums",
            column: "CountryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Players");

        migrationBuilder.DropTable(
            name: "Clubs");

        migrationBuilder.DropTable(
            name: "Stadiums");

        migrationBuilder.DropTable(
            name: "Countries");
    }
}
