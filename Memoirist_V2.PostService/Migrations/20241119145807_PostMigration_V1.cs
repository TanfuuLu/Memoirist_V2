using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memoirist_V2.PostService.Migrations
{
    /// <inheritdoc />
    public partial class PostMigration_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostWriterId = table.Column<int>(type: "integer", nullable: true),
                    PostWriterAvatar = table.Column<string>(type: "text", nullable: true),
                    PostWriterName = table.Column<string>(type: "text", nullable: true),
                    PostDateTime = table.Column<string>(type: "text", nullable: true),
                    PostContext = table.Column<string>(type: "text", nullable: true),
                    PostLike = table.Column<int>(type: "integer", nullable: true),
                    ListWriterLikePost = table.Column<List<int>>(type: "integer[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                });

            migrationBuilder.CreateTable(
                name: "CommentPosts",
                columns: table => new
                {
                    CommentPostId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CommentContext = table.Column<string>(type: "text", nullable: true),
                    CommentWriterId = table.Column<int>(type: "integer", nullable: true),
                    CommentWriterAvatar = table.Column<string>(type: "text", nullable: true),
                    CommentWriterName = table.Column<string>(type: "text", nullable: true),
                    CommentDate = table.Column<string>(type: "text", nullable: true),
                    CommentLike = table.Column<int>(type: "integer", nullable: true),
                    PostId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPosts", x => x.CommentPostId);
                    table.ForeignKey(
                        name: "FK_CommentPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentPosts_PostId",
                table: "CommentPosts",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPosts");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
