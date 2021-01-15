using Microsoft.EntityFrameworkCore.Migrations;

namespace ApI.Migrations
{
    public partial class UpdateChartTotalField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Charts",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Total",
                table: "Charts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
