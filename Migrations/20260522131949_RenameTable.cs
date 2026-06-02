using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccount.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_accounts_account_id",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "student_profiles");

            migrationBuilder.RenameIndex(
                name: "IX_students_account_id",
                table: "student_profiles",
                newName: "IX_student_profiles_account_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student_profiles",
                table: "student_profiles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_profiles_accounts_account_id",
                table: "student_profiles",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_profiles_accounts_account_id",
                table: "student_profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student_profiles",
                table: "student_profiles");

            migrationBuilder.RenameTable(
                name: "student_profiles",
                newName: "students");

            migrationBuilder.RenameIndex(
                name: "IX_student_profiles_account_id",
                table: "students",
                newName: "IX_students_account_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_accounts_account_id",
                table: "students",
                column: "account_id",
                principalTable: "accounts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
