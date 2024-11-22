using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseMovieTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "movie_tickets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_id",
                table: "movie_tickets");
        }
    }
}
