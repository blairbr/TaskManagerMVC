using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagerMVC.Migrations
{
    public partial class ReplacedNullableBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CompletionStatus",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "CompletionStatus",
                table: "Tasks",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
