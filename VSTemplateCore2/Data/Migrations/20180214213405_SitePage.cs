using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VSTemplateCore2.Data.Migrations {
	public partial class SitePage : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropIndex(
					name: "UserNameIndex",
					table: "AspNetUsers");

			migrationBuilder.DropIndex(
					name: "IX_AspNetUserRoles_UserId",
					table: "AspNetUserRoles");

			migrationBuilder.DropIndex(
					name: "RoleNameIndex",
					table: "AspNetRoles");

			migrationBuilder.CreateTable(
					name: "BannerImage",
					columns: table => new {
						Id = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						ButtonLink = table.Column<string>(maxLength: 255, nullable: true),
						ButtonText = table.Column<string>(maxLength: 25, nullable: true),
						ImagePath = table.Column<string>(maxLength: 255, nullable: false),
						Message = table.Column<string>(maxLength: 255, nullable: true),
						PhotoCredit = table.Column<string>(maxLength: 100, nullable: true)
					},
					constraints: table => {
						table.PrimaryKey("PK_BannerImage", x => x.Id);
					});

			migrationBuilder.CreateTable(
					name: "SitePage",
					columns: table => new {
						Id = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						Active = table.Column<bool>(nullable: false),
						Alias = table.Column<string>(maxLength: 65, nullable: true),
						AllowDelete = table.Column<bool>(nullable: false),
						AllowEdit = table.Column<bool>(nullable: false),
						Content = table.Column<string>(nullable: true),
						CreatedBy = table.Column<string>(maxLength: 36, nullable: true),
						CreatedOn = table.Column<DateTime>(nullable: false),
						HandlerAction = table.Column<string>(maxLength: 50, nullable: true),
						HandlerController = table.Column<string>(maxLength: 50, nullable: true),
						LastUpdatedBy = table.Column<string>(maxLength: 36, nullable: true),
						LastUpdatedOn = table.Column<DateTime>(nullable: false),
						MenuOrder = table.Column<string>(maxLength: 15, nullable: true),
						MenuTitle = table.Column<string>(maxLength: 30, nullable: true),
						MetaDescription = table.Column<string>(maxLength: 155, nullable: true),
						Name = table.Column<string>(maxLength: 65, nullable: true),
						PageBannerId = table.Column<int>(nullable: true),
						PageOrder = table.Column<int>(nullable: false),
						ParentMenuId = table.Column<int>(nullable: true),
						ShowInMenu = table.Column<bool>(nullable: false),
						Title = table.Column<string>(maxLength: 55, nullable: false)
					},
					constraints: table => {
						table.PrimaryKey("PK_SitePage", x => x.Id);
						table.ForeignKey(
											name: "FK_SitePage_BannerImage_PageBannerId",
											column: x => x.PageBannerId,
											principalTable: "BannerImage",
											principalColumn: "Id",
											onDelete: ReferentialAction.Restrict);
					});

			migrationBuilder.CreateIndex(
					name: "UserNameIndex",
					table: "AspNetUsers",
					column: "NormalizedUserName",
					unique: true,
					filter: "[NormalizedUserName] IS NOT NULL");

			migrationBuilder.CreateIndex(
					name: "RoleNameIndex",
					table: "AspNetRoles",
					column: "NormalizedName",
					unique: true,
					filter: "[NormalizedName] IS NOT NULL");

			migrationBuilder.CreateIndex(
					name: "IX_SitePage_PageBannerId",
					table: "SitePage",
					column: "PageBannerId");

			migrationBuilder.AddForeignKey(
					name: "FK_AspNetUserTokens_AspNetUsers_UserId",
					table: "AspNetUserTokens",
					column: "UserId",
					principalTable: "AspNetUsers",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropForeignKey(
					name: "FK_AspNetUserTokens_AspNetUsers_UserId",
					table: "AspNetUserTokens");

			migrationBuilder.DropTable(
					name: "SitePage");

			migrationBuilder.DropTable(
					name: "BannerImage");

			migrationBuilder.DropIndex(
					name: "UserNameIndex",
					table: "AspNetUsers");

			migrationBuilder.DropIndex(
					name: "RoleNameIndex",
					table: "AspNetRoles");

			migrationBuilder.CreateIndex(
					name: "UserNameIndex",
					table: "AspNetUsers",
					column: "NormalizedUserName",
					unique: true);

			migrationBuilder.CreateIndex(
					name: "IX_AspNetUserRoles_UserId",
					table: "AspNetUserRoles",
					column: "UserId");

			migrationBuilder.CreateIndex(
					name: "RoleNameIndex",
					table: "AspNetRoles",
					column: "NormalizedName");
		}
	}
}
