using Microsoft.EntityFrameworkCore.Migrations;

namespace IQTest.DataAccess.Migrations
{
    public partial class addNumberToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionImage",
                table: "Questions",
                newName: "Image");

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Questions",
                newName: "QuestionImage");
        }
    }
}
