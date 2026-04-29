using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health.Infrastructure.Adapters.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Plano_0855 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hth_exclusao_beneficiario_fila",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    beneficiario_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    prioridade = table.Column<int>(type: "integer", nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_exclusao_beneficiario_fila", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hth_planos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    codigo_registro_ans = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_planos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hth_beneficiarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    nome_completo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    cpf = table.Column<string>(type: "character(11)", fixedLength: true, maxLength: 11, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plano_id = table.Column<Guid>(type: "uuid", nullable: false),
                    data_cadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_beneficiarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_beneficiarios_planos_plano_id",
                        column: x => x.plano_id,
                        principalTable: "hth_planos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_beneficiarios_cpf",
                table: "hth_beneficiarios",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_beneficiarios_plano_id",
                table: "hth_beneficiarios",
                column: "plano_id");

            migrationBuilder.CreateIndex(
                name: "ix_fila_exclusao_prioridade_data",
                table: "hth_exclusao_beneficiario_fila",
                columns: new[] { "prioridade", "data_solicitacao" });

            migrationBuilder.CreateIndex(
                name: "ix_planos_codigo_registro_ans",
                table: "hth_planos",
                column: "codigo_registro_ans",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_planos_nome",
                table: "hth_planos",
                column: "nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hth_beneficiarios");

            migrationBuilder.DropTable(
                name: "hth_exclusao_beneficiario_fila");

            migrationBuilder.DropTable(
                name: "hth_planos");
        }
    }
}
