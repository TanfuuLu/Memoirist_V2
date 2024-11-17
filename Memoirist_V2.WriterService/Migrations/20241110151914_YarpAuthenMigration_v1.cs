using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memoirist_V2.WriterService.Migrations
{
    /// <inheritdoc />
    public partial class YarpAuthenMigration_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Writers",
                columns: table => new
                {
                    WriterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WriterFullname = table.Column<string>(type: "text", nullable: true),
                    WriterUsername = table.Column<string>(type: "text", nullable: true),
                    WriterAvatar = table.Column<string>(type: "text", nullable: true),
                    Account = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    WriterBio = table.Column<string>(type: "text", nullable: true),
                    WriterGender = table.Column<string>(type: "text", nullable: true),
                    WriterBirthday = table.Column<string>(type: "text", nullable: true),
                    WriterPhone = table.Column<string>(type: "text", nullable: true),
                    WriterEmail = table.Column<string>(type: "text", nullable: true),
                    ListPostId = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ListFollowingStoryId = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ListStoryId = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ListFollower = table.Column<List<int>>(type: "integer[]", nullable: true),
                    ListFollowing = table.Column<List<int>>(type: "integer[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writers", x => x.WriterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Writers");
        }
    }
}
