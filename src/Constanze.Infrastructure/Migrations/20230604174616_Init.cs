// Copyright (c) 2023 Quetzal Rivera.
// Licensed under the GNU General Public License v3.0, See LICENCE in the project root for license information.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Constanze.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Users",
			columns: table => new
			{
				Key = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Id = table.Column<long>(type: "bigint", nullable: false),
				FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
				LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Users", x => x.Key);
			});

		migrationBuilder.CreateTable(
			name: "Conversations",
			columns: table => new
			{
				Id = table.Column<long>(type: "bigint", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				IsClosed = table.Column<bool>(type: "bit", nullable: false),
				ChatId = table.Column<long>(type: "bigint", nullable: false),
				UserKey = table.Column<int>(type: "int", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Conversations", x => x.Id);
				table.ForeignKey(
					name: "FK_Conversations_Users_UserKey",
					column: x => x.UserKey,
					principalTable: "Users",
					principalColumn: "Key",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "DialogMessages",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				User = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Bot = table.Column<string>(type: "nvarchar(max)", nullable: true),
				ConversationId = table.Column<long>(type: "bigint", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_DialogMessages", x => x.Id);
				table.ForeignKey(
					name: "FK_DialogMessages_Conversations_ConversationId",
					column: x => x.ConversationId,
					principalTable: "Conversations",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Conversations_UserKey",
			table: "Conversations",
			column: "UserKey");

		migrationBuilder.CreateIndex(
			name: "IX_DialogMessages_ConversationId",
			table: "DialogMessages",
			column: "ConversationId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "DialogMessages");

		migrationBuilder.DropTable(
			name: "Conversations");

		migrationBuilder.DropTable(
			name: "Users");
	}
}
