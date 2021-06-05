using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeManagement.Migrations
{
    public partial class UpdateForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Terms_ID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Courses_ID",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CourseID",
                table: "Events",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TermID",
                table: "Courses",
                column: "TermID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Terms_TermID",
                table: "Courses",
                column: "TermID",
                principalTable: "Terms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Courses_CourseID",
                table: "Events",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Terms_TermID",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Courses_CourseID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_CourseID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TermID",
                table: "Courses");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Terms_ID",
                table: "Courses",
                column: "ID",
                principalTable: "Terms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Courses_ID",
                table: "Events",
                column: "ID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
