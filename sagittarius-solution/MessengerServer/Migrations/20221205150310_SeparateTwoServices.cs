using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversedService.Migrations
{
    /// <inheritdoc />
    public partial class SeparateTwoServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AuthorizationPairs_Id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AuthorizationPairs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateTable(
                name: "AuthorizationPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationPairs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AuthorizationPairs_Id",
                table: "Users",
                column: "Id",
                principalTable: "AuthorizationPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
