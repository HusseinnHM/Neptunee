using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Neptunee.EntityFrameworkCore.MultiLanguage.Types;

#nullable disable

namespace Sample.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventManagers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UtcDateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UtcDateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UtcDateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventManagers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    UtcDateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UtcDateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UtcDateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipationUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<MultiLanguageProperty>(type: "jsonb", nullable: false),
                    Description = table.Column<MultiLanguageProperty>(type: "jsonb", nullable: false),
                    Location = table.Column<MultiLanguageProperty>(type: "jsonb", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvailableTickets = table.Column<int>(type: "integer", nullable: false),
                    EventManagerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false),
                    UtcDateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UtcDateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UtcDateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventManagers_EventManagerId",
                        column: x => x.EventManagerId,
                        principalTable: "EventManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipationUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UtcDateDeleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UtcDateCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UtcDateUpdated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_ParticipationUsers_ParticipationUserId",
                        column: x => x.ParticipationUserId,
                        principalTable: "ParticipationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventManagers_Email",
                table: "EventManagers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventManagerId",
                table: "Events",
                column: "EventManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDate",
                table: "Events",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipationUsers_Email",
                table: "ParticipationUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParticipationUserId",
                table: "Tickets",
                column: "ParticipationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ParticipationUsers");

            migrationBuilder.DropTable(
                name: "EventManagers");
        }
    }
}
