using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCore.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Weapons",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Weapons",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Weapons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Factions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Factions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Factions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Factions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Characters",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Characters",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Backpacks",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Backpacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                table: "Backpacks",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Backpacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "ModifiedAt", "ModifiedBy" },
                values: new object[] { null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Factions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Factions");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Factions");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Factions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Backpacks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Backpacks");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Backpacks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Backpacks");
        }
    }
}
