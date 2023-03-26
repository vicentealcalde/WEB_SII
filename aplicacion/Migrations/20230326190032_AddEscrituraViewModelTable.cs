using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aplicacion.Migrations
{
    /// <inheritdoc />
    public partial class AddEscrituraViewModelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Escritura",
                columns: table => new
                {
                    NumAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CNE = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Comuna = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Manzana = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Predio = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Fojas = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "date", nullable: false),
                    NumeroInscripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Escritur__C0692259767AB5C6", x => x.NumAtencion);
                });

            migrationBuilder.CreateTable(
                name: "MULTIPROPIETARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comuna = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Manzana = table.Column<int>(type: "int", nullable: false),
                    Predio = table.Column<int>(type: "int", nullable: false),
                    RUN_RUT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PorcentajeDerecho = table.Column<double>(type: "float", nullable: false),
                    Fojas = table.Column<int>(type: "int", nullable: false),
                    AnoInscripcion = table.Column<int>(type: "int", nullable: false),
                    NumeroInscripcion = table.Column<int>(type: "int", nullable: false),
                    FechaInscripcion = table.Column<DateTime>(type: "date", nullable: false),
                    AnoVigenciaInicial = table.Column<int>(type: "int", nullable: false),
                    AnoVigenciaFinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MULTIPRO__3214EC07275953BA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adquirente",
                columns: table => new
                {
                    IdAdquirente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumAtencion = table.Column<int>(type: "int", nullable: false),
                    RUN_RUT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PorcentajeDerecho = table.Column<double>(type: "float", nullable: false),
                    PorcentajeDerechoNoAcreditado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Adquiren__AAEC4FDB48E843BF", x => x.IdAdquirente);
                    table.ForeignKey(
                        name: "FK__Adquirent__NumAt__3C69FB99",
                        column: x => x.NumAtencion,
                        principalTable: "Escritura",
                        principalColumn: "NumAtencion");
                });

            migrationBuilder.CreateTable(
                name: "Enajenante",
                columns: table => new
                {
                    IdEnajenante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumAtencion = table.Column<int>(type: "int", nullable: false),
                    RUN_RUT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PorcentajeDerecho = table.Column<double>(type: "float", nullable: false),
                    PorcentajeDerechoNoAcreditado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Enajenan__2664FAC4DDAF1CA5", x => x.IdEnajenante);
                    table.ForeignKey(
                        name: "FK__Enajenant__NumAt__398D8EEE",
                        column: x => x.NumAtencion,
                        principalTable: "Escritura",
                        principalColumn: "NumAtencion");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adquirente_NumAtencion",
                table: "Adquirente",
                column: "NumAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_Enajenante_NumAtencion",
                table: "Enajenante",
                column: "NumAtencion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adquirente");

            migrationBuilder.DropTable(
                name: "Enajenante");

            migrationBuilder.DropTable(
                name: "MULTIPROPIETARIO");

            migrationBuilder.DropTable(
                name: "Escritura");
        }
    }
}
