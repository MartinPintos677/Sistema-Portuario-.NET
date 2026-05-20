using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaPortuario.Migrations
{
    /// <inheritdoc />
    public partial class AddEmpresaScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "PersonalEstiba",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "LiquidacionEstiba",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Cuadrilla",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Cliente",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "CitacionEstiba",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalEstiba_IdEmpresa",
                table: "PersonalEstiba",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidacionEstiba_IdEmpresa",
                table: "LiquidacionEstiba",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Cuadrilla_IdEmpresa",
                table: "Cuadrilla",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_IdEmpresa",
                table: "Cliente",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_CitacionEstiba_IdEmpresa",
                table: "CitacionEstiba",
                column: "IdEmpresa");

            migrationBuilder.AddForeignKey(
                name: "FK_CitacionEstiba_Empresa_IdEmpresa",
                table: "CitacionEstiba",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Empresa_IdEmpresa",
                table: "Cliente",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cuadrilla_Empresa_IdEmpresa",
                table: "Cuadrilla",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LiquidacionEstiba_Empresa_IdEmpresa",
                table: "LiquidacionEstiba",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalEstiba_Empresa_IdEmpresa",
                table: "PersonalEstiba",
                column: "IdEmpresa",
                principalTable: "Empresa",
                principalColumn: "IdEmpresa",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CitacionEstiba_Empresa_IdEmpresa",
                table: "CitacionEstiba");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Empresa_IdEmpresa",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cuadrilla_Empresa_IdEmpresa",
                table: "Cuadrilla");

            migrationBuilder.DropForeignKey(
                name: "FK_LiquidacionEstiba_Empresa_IdEmpresa",
                table: "LiquidacionEstiba");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalEstiba_Empresa_IdEmpresa",
                table: "PersonalEstiba");

            migrationBuilder.DropIndex(
                name: "IX_PersonalEstiba_IdEmpresa",
                table: "PersonalEstiba");

            migrationBuilder.DropIndex(
                name: "IX_LiquidacionEstiba_IdEmpresa",
                table: "LiquidacionEstiba");

            migrationBuilder.DropIndex(
                name: "IX_Cuadrilla_IdEmpresa",
                table: "Cuadrilla");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_IdEmpresa",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_CitacionEstiba_IdEmpresa",
                table: "CitacionEstiba");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "PersonalEstiba");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "LiquidacionEstiba");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Cuadrilla");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "CitacionEstiba");
        }
    }
}
