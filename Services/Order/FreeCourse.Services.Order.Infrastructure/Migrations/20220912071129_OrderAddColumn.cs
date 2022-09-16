using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeCourse.Services.Order.Infrastructure.Migrations
{
    public partial class OrderAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailMessage",
                schema: "Ordering",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Ordering",
                table: "Orders",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailMessage",
                schema: "Ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Ordering",
                table: "Orders");
        }
    }
}
