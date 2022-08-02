using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceAppWeb.Migrations
{
    public partial class addedUserNameInTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Transactions");
        }
    }
}
