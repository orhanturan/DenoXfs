using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DenoXfs.Sim.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeviceStatePerDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Device state is per device now; the old rows (reader settings, retain count per terminal) have no device.
            migrationBuilder.Sql("DELETE FROM deno_device_state;");

            migrationBuilder.DropIndex(
                name: "ix_deno_xfs_service_model_device_id_xfs_class",
                table: "deno_xfs_service");

            migrationBuilder.DropIndex(
                name: "ix_deno_device_state_terminal_id_xfs_class",
                table: "deno_device_state");

            migrationBuilder.AddColumn<int>(
                name: "model_device_id",
                table: "deno_device_state",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_deno_xfs_service_model_device_id",
                table: "deno_xfs_service",
                column: "model_device_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_state_model_device_id",
                table: "deno_device_state",
                column: "model_device_id");

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_state_terminal_id_model_device_id_xfs_class",
                table: "deno_device_state",
                columns: new[] { "terminal_id", "model_device_id", "xfs_class" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_deno_device_state_model_device_id",
                table: "deno_device_state",
                column: "model_device_id",
                principalTable: "deno_model_device",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_deno_device_state_model_device_id",
                table: "deno_device_state");

            migrationBuilder.DropIndex(
                name: "ix_deno_xfs_service_model_device_id",
                table: "deno_xfs_service");

            migrationBuilder.DropIndex(
                name: "ix_deno_device_state_model_device_id",
                table: "deno_device_state");

            migrationBuilder.DropIndex(
                name: "ix_deno_device_state_terminal_id_model_device_id_xfs_class",
                table: "deno_device_state");

            migrationBuilder.DropColumn(
                name: "model_device_id",
                table: "deno_device_state");

            migrationBuilder.CreateIndex(
                name: "ix_deno_xfs_service_model_device_id_xfs_class",
                table: "deno_xfs_service",
                columns: new[] { "model_device_id", "xfs_class" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_deno_device_state_terminal_id_xfs_class",
                table: "deno_device_state",
                columns: new[] { "terminal_id", "xfs_class" },
                unique: true);
        }
    }
}
