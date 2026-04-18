using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPlanner.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocationAndRenameStoryThread : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Locations_LocationId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Threads_StoryThreadId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPoints_Locations_LocationId",
                table: "PlotPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThreads_Threads_StoryThreadId",
                table: "PlotPointThreads");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads");

            migrationBuilder.DropIndex(
                name: "IX_PlotPoints_LocationId",
                table: "PlotPoints");

            migrationBuilder.DropIndex(
                name: "IX_Notes_LocationId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Threads",
                table: "Threads");

            // Drop the old StoryThreadId FK column before renaming ThreadId to StoryThreadId
            migrationBuilder.DropColumn(
                name: "StoryThreadId",
                table: "PlotPointThreads");

            // Now this rename is safe — no duplicate
            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "PlotPointThreads",
                newName: "StoryThreadId");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "PlotPoints");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "Threads",
                newName: "StoryThreads");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads",
                columns: new[] { "PlotPointId", "StoryThreadId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryThreads",
                table: "StoryThreads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_StoryThreads_StoryThreadId",
                table: "Notes",
                column: "StoryThreadId",
                principalTable: "StoryThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThreads_StoryThreads_StoryThreadId",
                table: "PlotPointThreads",
                column: "StoryThreadId",
                principalTable: "StoryThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_StoryThreads_StoryThreadId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotPointThreads_StoryThreads_StoryThreadId",
                table: "PlotPointThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryThreads",
                table: "StoryThreads");

            migrationBuilder.RenameTable(
                name: "StoryThreads",
                newName: "Threads");


            // Reverse: rename StoryThreadId back to ThreadId
            migrationBuilder.RenameColumn(
                name: "StoryThreadId",
                table: "PlotPointThreads",
                newName: "ThreadId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "PlotPoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoryThreadId",
                table: "PlotPointThreads",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotPointThreads",
                table: "PlotPointThreads",
                columns: new[] { "PlotPointId", "ThreadId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Threads",
                table: "Threads",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Region = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlotPoints_LocationId",
                table: "PlotPoints",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_LocationId",
                table: "Notes",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Locations_LocationId",
                table: "Notes",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Threads_StoryThreadId",
                table: "Notes",
                column: "StoryThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPoints_Locations_LocationId",
                table: "PlotPoints",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlotPointThreads_Threads_StoryThreadId",
                table: "PlotPointThreads",
                column: "StoryThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
