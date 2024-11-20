using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memoirist_V2.StoryService.Migrations
{
    /// <inheritdoc />
    public partial class StoryMigration_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    StoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WriterStoryId = table.Column<int>(type: "integer", nullable: true),
                    StoryName = table.Column<string>(type: "text", nullable: true),
                    StoryIntroduction = table.Column<string>(type: "text", nullable: true),
                    StoryAuthor = table.Column<string>(type: "text", nullable: true),
                    StoryLikes = table.Column<int>(type: "integer", nullable: true),
                    StoryPicture = table.Column<string>(type: "text", nullable: true),
                    StoryDateWrited = table.Column<string>(type: "text", nullable: true),
                    TermsAndConditionsCheck = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.StoryId);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    ChapterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StoryId = table.Column<int>(type: "integer", nullable: false),
                    ChapterTitle = table.Column<string>(type: "text", nullable: true),
                    ChapterContext = table.Column<string>(type: "text", nullable: true),
                    ChapterNumber = table.Column<int>(type: "integer", nullable: true),
                    ChapterDateTime = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.ChapterId);
                    table.ForeignKey(
                        name: "FK_Chapters_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentWriterId = table.Column<int>(type: "integer", nullable: true),
                    CommentLike = table.Column<int>(type: "integer", nullable: true),
                    CommentContext = table.Column<string>(type: "text", nullable: true),
                    CommentDateTime = table.Column<string>(type: "text", nullable: true),
                    StoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_StoryId",
                table: "Chapters",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StoryId",
                table: "Comments",
                column: "StoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}
