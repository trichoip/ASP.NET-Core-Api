using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCore.EntityFramework.Migrations;

/// <inheritdoc />
public partial class init3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Characters",
            type: "nvarchar(165)",
            maxLength: 165,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Characters",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(165)",
            oldMaxLength: 165);
    }
}
