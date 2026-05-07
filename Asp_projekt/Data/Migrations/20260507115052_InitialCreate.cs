using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asp_projekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fakulteti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakulteti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kolegiji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ECTS = table.Column<int>(type: "int", nullable: false),
                    FakultetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kolegiji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kolegiji_Fakulteti_FakultetId",
                        column: x => x.FakultetId,
                        principalTable: "Fakulteti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profesori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Katedra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FakultetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profesori_Fakulteti_FakultetId",
                        column: x => x.FakultetId,
                        principalTable: "Fakulteti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumUpisa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FakultetId = table.Column<int>(type: "int", nullable: true),
                    KolegijId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_Fakulteti_FakultetId",
                        column: x => x.FakultetId,
                        principalTable: "Fakulteti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Studenti_Kolegiji_KolegijId",
                        column: x => x.KolegijId,
                        principalTable: "Kolegiji",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Izvjestaji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    ProsjecnaOcjena = table.Column<double>(type: "float", nullable: false),
                    BrojOcjena = table.Column<int>(type: "int", nullable: false),
                    DatumGeneriranja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvjestaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Izvjestaji_Profesori_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KolegijProfesor",
                columns: table => new
                {
                    KolegijiId = table.Column<int>(type: "int", nullable: false),
                    ProfesoriId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KolegijProfesor", x => new { x.KolegijiId, x.ProfesoriId });
                    table.ForeignKey(
                        name: "FK_KolegijProfesor_Kolegiji_KolegijiId",
                        column: x => x.KolegijiId,
                        principalTable: "Kolegiji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KolegijProfesor_Profesori_ProfesoriId",
                        column: x => x.ProfesoriId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocjene",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vrijednost = table.Column<int>(type: "int", nullable: false),
                    Komentar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumOcjene = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    KolegijId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocjene", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ocjene_Kolegiji_KolegijId",
                        column: x => x.KolegijId,
                        principalTable: "Kolegiji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ocjene_Profesori_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ocjene_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izvjestaji_ProfesorId",
                table: "Izvjestaji",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Kolegiji_FakultetId",
                table: "Kolegiji",
                column: "FakultetId");

            migrationBuilder.CreateIndex(
                name: "IX_KolegijProfesor_ProfesoriId",
                table: "KolegijProfesor",
                column: "ProfesoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjene_KolegijId",
                table: "Ocjene",
                column: "KolegijId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjene_ProfesorId",
                table: "Ocjene",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjene_StudentId",
                table: "Ocjene",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesori_FakultetId",
                table: "Profesori",
                column: "FakultetId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_FakultetId",
                table: "Studenti",
                column: "FakultetId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_KolegijId",
                table: "Studenti",
                column: "KolegijId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izvjestaji");

            migrationBuilder.DropTable(
                name: "KolegijProfesor");

            migrationBuilder.DropTable(
                name: "Ocjene");

            migrationBuilder.DropTable(
                name: "Profesori");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Kolegiji");

            migrationBuilder.DropTable(
                name: "Fakulteti");
        }
    }
}
