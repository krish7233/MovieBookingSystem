using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_movies_MovieId",
                table: "bookings");

            migrationBuilder.CreateTable(
                name: "Cast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CastMovie",
                columns: table => new
                {
                    CastsId = table.Column<int>(type: "int", nullable: false),
                    moviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastMovie", x => new { x.CastsId, x.moviesId });
                    table.ForeignKey(
                        name: "FK_CastMovie_Cast_CastsId",
                        column: x => x.CastsId,
                        principalTable: "Cast",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CastMovie_movies_moviesId",
                        column: x => x.moviesId,
                        principalTable: "movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CastMovie_moviesId",
                table: "CastMovie",
                column: "moviesId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_movies_MovieId",
                table: "bookings",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_movies_MovieId",
                table: "bookings");

            migrationBuilder.DropTable(
                name: "CastMovie");

            migrationBuilder.DropTable(
                name: "Cast");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_movies_MovieId",
                table: "bookings",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
