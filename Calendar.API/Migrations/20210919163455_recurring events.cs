using Microsoft.EntityFrameworkCore.Migrations;

namespace Calendar.API.Migrations
{
    public partial class recurringevents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequiredAttendees",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<long>(
                name: "EventLength",
                table: "Events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "EventPID",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RecurrenceType",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventLength",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventPID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RecurrenceType",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "RequiredAttendees",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
