using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAccount.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablesTeacherDiscipline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_profiles_groups_group_id",
                table: "student_profiles");

            migrationBuilder.CreateTable(
                name: "disciplines",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disciplines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teacher_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    account_id = table.Column<int>(type: "INTEGER", nullable: false),
                    full_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    photo_url = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_profiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_profiles_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_group_disciplines",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    group_id = table.Column<int>(type: "INTEGER", nullable: false),
                    discipline_id = table.Column<int>(type: "INTEGER", nullable: false),
                    teacher_account_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_group_disciplines", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_group_disciplines_accounts_teacher_account_id",
                        column: x => x.teacher_account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_group_disciplines_disciplines_discipline_id",
                        column: x => x.discipline_id,
                        principalTable: "disciplines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_group_disciplines_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teacher_group_disciplines_discipline_id",
                table: "teacher_group_disciplines",
                column: "discipline_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_group_disciplines_group_id",
                table: "teacher_group_disciplines",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_group_disciplines_teacher_account_id",
                table: "teacher_group_disciplines",
                column: "teacher_account_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_profiles_account_id",
                table: "teacher_profiles",
                column: "account_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_student_profiles_groups_group_id",
                table: "student_profiles",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_profiles_groups_group_id",
                table: "student_profiles");

            migrationBuilder.DropTable(
                name: "teacher_group_disciplines");

            migrationBuilder.DropTable(
                name: "teacher_profiles");

            migrationBuilder.DropTable(
                name: "disciplines");

            migrationBuilder.AddForeignKey(
                name: "FK_student_profiles_groups_group_id",
                table: "student_profiles",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
