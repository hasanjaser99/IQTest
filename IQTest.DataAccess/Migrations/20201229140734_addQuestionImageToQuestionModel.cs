using Microsoft.EntityFrameworkCore.Migrations;

namespace IQTest.DataAccess.Migrations
{
    public partial class addQuestionImageToQuestionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestionImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionImage",
                table: "Questions");
        }
    }
}
