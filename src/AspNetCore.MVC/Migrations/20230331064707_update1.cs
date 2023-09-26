using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace asp.net_core_empty_5._0.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK3sr9aje2ymv15iu0kar1idwyt",
                table: "car");

            migrationBuilder.AddForeignKey(
                name: "FK3sr9aje2ymv15iu0kar1idwyt",
                table: "car",
                column: "id",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                name: "FK5ve989unrb6nk6duv2qt7hesc",
                table: "book");

            migrationBuilder.AddForeignKey(
                name: "FK5ve989unrb6nk6duv2qt7hesc",
                table: "book",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                name: "FKd86e0b4v70sx9onvqpf3hc81h",
                table: "car_feature");

            migrationBuilder.AddForeignKey(
                name: "FKd86e0b4v70sx9onvqpf3hc81h",
                table: "car_feature",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
                 name: "FK91nl01tvyj0xus5j92voo4ht1",
                 table: "car_image");

            migrationBuilder.AddForeignKey(
                name: "FK91nl01tvyj0xus5j92voo4ht1",
                table: "car_image",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);



            migrationBuilder.DropForeignKey(
                 name: "FKsgbdbp0r3xpuuk0143gnuqbqw",
                 table: "like_table");

            migrationBuilder.AddForeignKey(
                name: "FKsgbdbp0r3xpuuk0143gnuqbqw",
                table: "like_table",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);



            migrationBuilder.DropForeignKey(
                name: "FKdfvvph8r2jqrhy24rl70jggix",
                table: "review");

            migrationBuilder.AddForeignKey(
                name: "FKdfvvph8r2jqrhy24rl70jggix",
                table: "review",
                column: "id",
                principalTable: "book",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
               name: "FK3sr9aje2ymv15iu0kar1idwyt",
               table: "car");

            migrationBuilder.AddForeignKey(
                name: "FK3sr9aje2ymv15iu0kar1idwyt",
                table: "car",
                column: "id",
                principalTable: "address",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.DropForeignKey(
                name: "FK5ve989unrb6nk6duv2qt7hesc",
                table: "book");

            migrationBuilder.AddForeignKey(
                name: "FK5ve989unrb6nk6duv2qt7hesc",
                table: "book",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.DropForeignKey(
                name: "FKd86e0b4v70sx9onvqpf3hc81h",
                table: "car_feature");

            migrationBuilder.AddForeignKey(
                name: "FKd86e0b4v70sx9onvqpf3hc81h",
                table: "car_feature",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                 name: "FK91nl01tvyj0xus5j92voo4ht1",
                 table: "car_image");

            migrationBuilder.AddForeignKey(
                name: "FK91nl01tvyj0xus5j92voo4ht1",
                table: "car_image",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.DropForeignKey(
                 name: "FKsgbdbp0r3xpuuk0143gnuqbqw",
                 table: "like_table");

            migrationBuilder.AddForeignKey(
                name: "FKsgbdbp0r3xpuuk0143gnuqbqw",
                table: "like_table",
                column: "car_id",
                principalTable: "car",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);



            migrationBuilder.DropForeignKey(
                name: "FKdfvvph8r2jqrhy24rl70jggix",
                table: "review");

            migrationBuilder.AddForeignKey(
                name: "FKdfvvph8r2jqrhy24rl70jggix",
                table: "review",
                column: "id",
                principalTable: "book",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

        }
    }
}
