using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventsApplication.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    IsFull = table.Column<bool>(type: "bit", nullable: false),
                    EventImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0225d3f4-33d2-4068-8275-ee56e4681e3e"), "Mogilev Palace of Culture" },
                    { new Guid("0559587c-b36d-4d42-b0f7-68639bf4e951"), "Cathedral of St. Stanislaus" },
                    { new Guid("2d747b20-1a65-48cf-986a-2a97e557c34a"), "Thousand Years of Mogilev Square" },
                    { new Guid("41dd7358-2cc4-4953-b141-659f1a7b8eea"), "Mogilev Zoo" },
                    { new Guid("5b685bbd-051f-4bc1-9dbb-8cd4c38217d0"), "Museum of History of Mogilev" },
                    { new Guid("94d29087-2af2-44df-9484-d3d8b0a4c3ce"), "Europe Shopping Center" },
                    { new Guid("b8160578-9e2b-4885-bdc1-9b55618240ec"), "Atrium Shopping Center" },
                    { new Guid("d40dc058-d7c6-4dbf-92b8-ca9936a1362f"), "Olympic Sports Complex" },
                    { new Guid("d9cadc88-99da-4eea-8ce6-9af7431642e1"), "Victory Park" },
                    { new Guid("f131fdd7-e0ac-465f-8ec0-3056095ab06b"), "Central Park of Culture and Recreation" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Birthday", "Email", "FirstName", "IsActivated", "LastName", "PasswordHash", "UserRole" },
                values: new object[] { new Guid("43130ccf-faf1-4445-8f99-a54a1e661f5d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@gmail.com", "Admin", true, "Admin", "$2a$11$0fz1SsBAF8ZIC1nNAcJ0MOnrU8gQp1.CpP5oz5YD5OShgobb2wGAq", "Admin" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Category", "Description", "EventImageName", "EventTime", "IsFull", "MaxParticipants", "Name", "PlaceId" },
                values: new object[,]
                {
                    { new Guid("1c6de99a-34ac-4be9-8e61-c3b7aa7a0224"), "Food", "A delightful festival showcasing various cuisines.", "", new DateTime(2024, 10, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), false, 1000, "Food Festival", new Guid("0225d3f4-33d2-4068-8275-ee56e4681e3e") },
                    { new Guid("2b84fe47-9c93-43f8-b7df-2be289eb1bf1"), "Music", "A great music festival featuring various artists.", "", new DateTime(2024, 6, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), false, 50, "Music Festival", new Guid("d9cadc88-99da-4eea-8ce6-9af7431642e1") },
                    { new Guid("641db76b-c08d-46e4-9b72-8242662c66a3"), "Technology", "An insightful conference about the latest in technology.", "", new DateTime(2024, 9, 10, 9, 0, 0, 0, DateTimeKind.Unspecified), false, 30, "Tech Conference", new Guid("d9cadc88-99da-4eea-8ce6-9af7431642e1") },
                    { new Guid("a267be90-4e7f-4dd8-a55f-e62b5de45b82"), "Health", "Learn about healthy living and wellness practices.", "", new DateTime(2024, 8, 5, 15, 0, 0, 0, DateTimeKind.Unspecified), false, 15, "Health Workshop", new Guid("d9cadc88-99da-4eea-8ce6-9af7431642e1") },
                    { new Guid("bbf4fa4f-37c1-4fa4-88cf-8f02f67c8353"), "Art", "Explore the latest artworks from renowned artists.", "", new DateTime(2024, 7, 20, 10, 0, 0, 0, DateTimeKind.Unspecified), false, 20, "Art Exhibition", new Guid("0225d3f4-33d2-4068-8275-ee56e4681e3e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Category",
                table: "Events",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PlaceId",
                table: "Events",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EventId",
                table: "Subscriptions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Places");
        }
    }
}
