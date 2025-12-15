using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class ModelRefinements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SourceMaterialId",
                table: "SourceMaterials",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Intensity",
                table: "PlotPoints",
                newName: "TensionPhase");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PlotPoints",
                newName: "Synopsis");

            migrationBuilder.AddColumn<int>(
                name: "ThreadScope",
                table: "Threads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImpactDescription",
                table: "PlotPointThread",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "PlotPointThread",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ThreadTrajectory",
                table: "PlotPointThread",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConflictType",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoreDriver",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Outcome",
                table: "PlotPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Presentation",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Stakes",
                table: "PlotPoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DependencyType",
                table: "PlotPointDependency",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThreadScope",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "ImpactDescription",
                table: "PlotPointThread");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "PlotPointThread");

            migrationBuilder.DropColumn(
                name: "ThreadTrajectory",
                table: "PlotPointThread");

            migrationBuilder.DropColumn(
                name: "ConflictType",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "CoreDriver",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "Outcome",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "Presentation",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "Stakes",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "DependencyType",
                table: "PlotPointDependency");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SourceMaterials",
                newName: "SourceMaterialId");

            migrationBuilder.RenameColumn(
                name: "TensionPhase",
                table: "PlotPoints",
                newName: "Intensity");

            migrationBuilder.RenameColumn(
                name: "Synopsis",
                table: "PlotPoints",
                newName: "Description");
        }
    }
}
