using Microsoft.EntityFrameworkCore.Migrations;

namespace FreeCourse.Services.Order.Infrastructure.Migrations
{
    public partial class OrderCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "Ordering",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                schema: "Ordering",
                table: "OrderItems");
        }
    }
}
