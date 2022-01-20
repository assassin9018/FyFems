using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations;

public partial class ColumnsAdded : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "Dialogs",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "DialogId",
            table: "Contacts",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Contacts_DialogId",
            table: "Contacts",
            column: "DialogId");

        migrationBuilder.AddForeignKey(
            name: "FK_Contacts_Dialogs_DialogId",
            table: "Contacts",
            column: "DialogId",
            principalTable: "Dialogs",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Contacts_Dialogs_DialogId",
            table: "Contacts");

        migrationBuilder.DropIndex(
            name: "IX_Contacts_DialogId",
            table: "Contacts");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "Dialogs");

        migrationBuilder.DropColumn(
            name: "DialogId",
            table: "Contacts");
    }
}
