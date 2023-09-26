using Microsoft.EntityFrameworkCore.Migrations;

namespace asp.net_core_empty_5._0.Migrations
{
    public partial class cascadecarfeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKgqgv3iyd1518909jkijos3tg3",
                table: "car_feature");

            migrationBuilder.AddForeignKey(
                name: "FKgqgv3iyd1518909jkijos3tg3",
                table: "car_feature",
                column: "feature_id",
                principalTable: "feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FKgqgv3iyd1518909jkijos3tg3",
                table: "car_feature");

            migrationBuilder.AddForeignKey(
                name: "FKgqgv3iyd1518909jkijos3tg3",
                table: "car_feature",
                column: "feature_id",
                principalTable: "feature",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
