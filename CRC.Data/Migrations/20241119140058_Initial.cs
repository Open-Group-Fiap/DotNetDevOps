using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_OP_CRC_AUTH",
                columns: table => new
                {
                    ID_AUTH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HASH_SENHA = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_AUTH", x => x.ID_AUTH);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_CRC_CONDOMINIO",
                columns: table => new
                {
                    ID_CONDOMINIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ENDERECO = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_CONDOMINIO", x => x.ID_CONDOMINIO);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_CRC_BONUS",
                columns: table => new
                {
                    ID_BONUS = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_CONDOMINIO = table.Column<int>(type: "int", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTO = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QTD_MAX = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_BONUS", x => x.ID_BONUS);
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_BONUS_T_OP_CRC_CONDOMINIO_ID_CONDOMINIO",
                        column: x => x.ID_CONDOMINIO,
                        principalTable: "T_OP_CRC_CONDOMINIO",
                        principalColumn: "ID_CONDOMINIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_CRC_MORADOR",
                columns: table => new
                {
                    ID_MORADOR = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_CONDOMINIO = table.Column<int>(type: "int", nullable: false),
                    ID_AUTH = table.Column<int>(type: "int", nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PONTOS = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    QTD_MORADORES = table.Column<int>(type: "int", nullable: false),
                    IDENTIFICADOR_RES = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_MORADOR", x => x.ID_MORADOR);
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_MORADOR_T_OP_CRC_AUTH_ID_AUTH",
                        column: x => x.ID_AUTH,
                        principalTable: "T_OP_CRC_AUTH",
                        principalColumn: "ID_AUTH",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_MORADOR_T_OP_CRC_CONDOMINIO_ID_CONDOMINIO",
                        column: x => x.ID_CONDOMINIO,
                        principalTable: "T_OP_CRC_CONDOMINIO",
                        principalColumn: "ID_CONDOMINIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_CRC_FATURA",
                columns: table => new
                {
                    ID_FATURA = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_MORADOR = table.Column<int>(type: "int", nullable: false),
                    QTD_CONSUMIDA = table.Column<int>(type: "int", nullable: false),
                    DT_GERACAO = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_FATURA", x => x.ID_FATURA);
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_FATURA_T_OP_CRC_MORADOR_ID_MORADOR",
                        column: x => x.ID_MORADOR,
                        principalTable: "T_OP_CRC_MORADOR",
                        principalColumn: "ID_MORADOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_CRC_MORADOR_BONUS",
                columns: table => new
                {
                    ID_MB = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_MORADOR = table.Column<int>(type: "int", nullable: false),
                    ID_BONUS = table.Column<int>(type: "int", nullable: false),
                    QTD = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_CRC_MORADOR_BONUS", x => x.ID_MB);
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_MORADOR_BONUS_T_OP_CRC_BONUS_ID_BONUS",
                        column: x => x.ID_BONUS,
                        principalTable: "T_OP_CRC_BONUS",
                        principalColumn: "ID_BONUS");
                    table.ForeignKey(
                        name: "FK_T_OP_CRC_MORADOR_BONUS_T_OP_CRC_MORADOR_ID_MORADOR",
                        column: x => x.ID_MORADOR,
                        principalTable: "T_OP_CRC_MORADOR",
                        principalColumn: "ID_MORADOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_AUTH_EMAIL",
                table: "T_OP_CRC_AUTH",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_BONUS_ID_CONDOMINIO",
                table: "T_OP_CRC_BONUS",
                column: "ID_CONDOMINIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_FATURA_ID_MORADOR",
                table: "T_OP_CRC_FATURA",
                column: "ID_MORADOR");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_MORADOR_CPF_ID_AUTH",
                table: "T_OP_CRC_MORADOR",
                columns: new[] { "CPF", "ID_AUTH" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_MORADOR_ID_AUTH",
                table: "T_OP_CRC_MORADOR",
                column: "ID_AUTH",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_MORADOR_ID_CONDOMINIO",
                table: "T_OP_CRC_MORADOR",
                column: "ID_CONDOMINIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_MORADOR_BONUS_ID_BONUS",
                table: "T_OP_CRC_MORADOR_BONUS",
                column: "ID_BONUS");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_CRC_MORADOR_BONUS_ID_MORADOR",
                table: "T_OP_CRC_MORADOR_BONUS",
                column: "ID_MORADOR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_OP_CRC_FATURA");

            migrationBuilder.DropTable(
                name: "T_OP_CRC_MORADOR_BONUS");

            migrationBuilder.DropTable(
                name: "T_OP_CRC_BONUS");

            migrationBuilder.DropTable(
                name: "T_OP_CRC_MORADOR");

            migrationBuilder.DropTable(
                name: "T_OP_CRC_AUTH");

            migrationBuilder.DropTable(
                name: "T_OP_CRC_CONDOMINIO");
        }
    }
}
