﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PostChanges1127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Countries_CountryId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CityId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CountryId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Posts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Posts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Posts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Posts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CityId",
                table: "Posts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CountryId",
                table: "Posts",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Cities_CityId",
                table: "Posts",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Countries_CountryId",
                table: "Posts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
