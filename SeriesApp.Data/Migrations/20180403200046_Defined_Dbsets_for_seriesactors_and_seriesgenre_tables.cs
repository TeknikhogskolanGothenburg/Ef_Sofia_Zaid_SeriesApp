using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SeriesApp.Data.Migrations
{
    public partial class Defined_Dbsets_for_seriesactors_and_seriesgenre_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesActor_Actors_ActorId",
                table: "SeriesActor");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesActor_Series_SeriesId",
                table: "SeriesActor");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesGenre_Genres_GenreId",
                table: "SeriesGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesGenre_Series_SeriesId",
                table: "SeriesGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesGenre",
                table: "SeriesGenre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesActor",
                table: "SeriesActor");

            migrationBuilder.RenameTable(
                name: "SeriesGenre",
                newName: "SeriesGenres");

            migrationBuilder.RenameTable(
                name: "SeriesActor",
                newName: "SeriesActors");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesGenre_GenreId",
                table: "SeriesGenres",
                newName: "IX_SeriesGenres_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesActor_ActorId",
                table: "SeriesActors",
                newName: "IX_SeriesActors_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesGenres",
                table: "SeriesGenres",
                columns: new[] { "SeriesId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesActors",
                table: "SeriesActors",
                columns: new[] { "SeriesId", "ActorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesActors_Actors_ActorId",
                table: "SeriesActors",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesActors_Series_SeriesId",
                table: "SeriesActors",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesGenres_Genres_GenreId",
                table: "SeriesGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesGenres_Series_SeriesId",
                table: "SeriesGenres",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesActors_Actors_ActorId",
                table: "SeriesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesActors_Series_SeriesId",
                table: "SeriesActors");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesGenres_Genres_GenreId",
                table: "SeriesGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesGenres_Series_SeriesId",
                table: "SeriesGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesGenres",
                table: "SeriesGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesActors",
                table: "SeriesActors");

            migrationBuilder.RenameTable(
                name: "SeriesGenres",
                newName: "SeriesGenre");

            migrationBuilder.RenameTable(
                name: "SeriesActors",
                newName: "SeriesActor");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesGenres_GenreId",
                table: "SeriesGenre",
                newName: "IX_SeriesGenre_GenreId");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesActors_ActorId",
                table: "SeriesActor",
                newName: "IX_SeriesActor_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesGenre",
                table: "SeriesGenre",
                columns: new[] { "SeriesId", "GenreId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesActor",
                table: "SeriesActor",
                columns: new[] { "SeriesId", "ActorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesActor_Actors_ActorId",
                table: "SeriesActor",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesActor_Series_SeriesId",
                table: "SeriesActor",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesGenre_Genres_GenreId",
                table: "SeriesGenre",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesGenre_Series_SeriesId",
                table: "SeriesGenre",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
