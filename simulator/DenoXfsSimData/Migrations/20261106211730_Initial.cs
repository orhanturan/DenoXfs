using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DenoXfs.Sim.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:deno_ci", "und-u-ks-level2,und-u-ks-level2,icu,False");

            migrationBuilder.CreateTable(
                name: "deno_card",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    track1 = table.Column<string>(type: "text", nullable: true),
                    track2 = table.Column<string>(type: "text", nullable: true),
                    track3 = table.Column<string>(type: "text", nullable: true),
                    atr = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_card", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deno_device_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    xfs_class = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    json = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_device_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deno_profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kind = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false, collation: "deno_ci"),
                    description = table.Column<string>(type: "text", nullable: true),
                    based_on = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deno_vendor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false, collation: "deno_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_vendor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deno_card_chip_response",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    card_id = table.Column<int>(type: "integer", nullable: false),
                    command = table.Column<string>(type: "text", nullable: false),
                    response = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_card_chip_response", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_card_chip_response_card_id",
                        column: x => x.card_id,
                        principalTable: "deno_card",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deno_profile_section",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    xfs_class = table.Column<string>(type: "text", nullable: false),
                    json = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_profile_section", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_profile_section_profile_id",
                        column: x => x.profile_id,
                        principalTable: "deno_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deno_terminal_model",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vendor_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false, collation: "deno_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_terminal_model", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_terminal_model_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "deno_vendor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deno_model_device",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    device_type = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false, collation: "deno_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_model_device", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_model_device_model_id",
                        column: x => x.model_id,
                        principalTable: "deno_terminal_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deno_terminal",
                columns: table => new
                {
                    terminal_id = table.Column<string>(type: "character(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    address1 = table.Column<string>(type: "text", nullable: false),
                    address2 = table.Column<string>(type: "text", nullable: true),
                    model_id = table.Column<int>(type: "integer", nullable: false),
                    scenario_id = table.Column<int>(type: "integer", nullable: true),
                    device_profile_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_terminal", x => x.terminal_id);
                    table.ForeignKey(
                        name: "fk_deno_terminal_device_profile_id",
                        column: x => x.device_profile_id,
                        principalTable: "deno_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_deno_terminal_model_id",
                        column: x => x.model_id,
                        principalTable: "deno_terminal_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_deno_terminal_scenario_id",
                        column: x => x.scenario_id,
                        principalTable: "deno_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "deno_xfs_service",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model_device_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false, collation: "deno_ci"),
                    xfs_class = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_xfs_service", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_xfs_service_model_device_id",
                        column: x => x.model_device_id,
                        principalTable: "deno_model_device",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "deno_device_state",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    terminal_id = table.Column<string>(type: "character(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    model_device_id = table.Column<int>(type: "integer", nullable: false),
                    xfs_class = table.Column<string>(type: "text", nullable: false),
                    json = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deno_device_state", x => x.id);
                    table.ForeignKey(
                        name: "fk_deno_device_state_model_device_id",
                        column: x => x.model_device_id,
                        principalTable: "deno_model_device",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_deno_device_state_terminal_id",
                        column: x => x.terminal_id,
                        principalTable: "deno_terminal",
                        principalColumn: "terminal_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_deno_card_name",
                table: "deno_card",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_card_chip_response_card_id_command",
                table: "deno_card_chip_response",
                columns: new[] { "card_id", "command" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_data_xfs_class_category",
                table: "deno_device_data",
                columns: new[] { "xfs_class", "category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_state_model_device_id",
                table: "deno_device_state",
                column: "model_device_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_state_terminal_id_model_device_id_xfs_class",
                table: "deno_device_state",
                columns: new[] { "terminal_id", "model_device_id", "xfs_class" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_model_device_model_id_name",
                table: "deno_model_device",
                columns: new[] { "model_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_profile_kind_name",
                table: "deno_profile",
                columns: new[] { "kind", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_profile_section_profile_id_xfs_class",
                table: "deno_profile_section",
                columns: new[] { "profile_id", "xfs_class" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_address1",
                table: "deno_terminal",
                column: "address1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_address2",
                table: "deno_terminal",
                column: "address2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_device_profile_id",
                table: "deno_terminal",
                column: "device_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_model_id",
                table: "deno_terminal",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_scenario_id",
                table: "deno_terminal",
                column: "scenario_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_terminal_model_vendor_id_name",
                table: "deno_terminal_model",
                columns: new[] { "vendor_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_vendor_name",
                table: "deno_vendor",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_xfs_service_model_device_id",
                table: "deno_xfs_service",
                column: "model_device_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "deno_card_chip_response");

            migrationBuilder.DropTable(
                name: "deno_device_data");

            migrationBuilder.DropTable(
                name: "deno_device_state");

            migrationBuilder.DropTable(
                name: "deno_profile_section");

            migrationBuilder.DropTable(
                name: "deno_xfs_service");

            migrationBuilder.DropTable(
                name: "deno_card");

            migrationBuilder.DropTable(
                name: "deno_terminal");

            migrationBuilder.DropTable(
                name: "deno_model_device");

            migrationBuilder.DropTable(
                name: "deno_profile");

            migrationBuilder.DropTable(
                name: "deno_terminal_model");

            migrationBuilder.DropTable(
                name: "deno_vendor");
        }
    }
}
