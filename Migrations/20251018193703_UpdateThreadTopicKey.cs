using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebForum.Migrations
{
    /// <inheritdoc />
    public partial class UpdateThreadTopicKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadTopics_Threads_TheadId",
                table: "ThreadTopics");

            migrationBuilder.RenameColumn(
                name: "TheadId",
                table: "ThreadTopics",
                newName: "ThreadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadTopics_Threads_ThreadId",
                table: "ThreadTopics",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ThreadTopics_Threads_ThreadId",
                table: "ThreadTopics");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "ThreadTopics",
                newName: "TheadId");

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadTopics_Threads_TheadId",
                table: "ThreadTopics",
                column: "TheadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
