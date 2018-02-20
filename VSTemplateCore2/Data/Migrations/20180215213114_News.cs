using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VSTemplateCore2.Data.Migrations {
	public partial class News : Migration {
		protected override void Up(MigrationBuilder migrationBuilder) {
			migrationBuilder.CreateTable(
					name: "News",
					columns: table => new {
						Id = table.Column<int>(nullable: false)
									.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
						Active = table.Column<bool>(nullable: false),
						Alias = table.Column<string>(maxLength: 65, nullable: false),
						ArticleDate = table.Column<DateTime>(nullable: false),
						Content = table.Column<string>(nullable: true),
						DisplayOnDate = table.Column<DateTime>(nullable: false),
						Excerpt = table.Column<string>(maxLength: 255, nullable: true),
						Featured = table.Column<bool>(nullable: false),
						ImagePath = table.Column<string>(maxLength: 255, nullable: true),
						LastUpdatedBy = table.Column<string>(maxLength: 36, nullable: true),
						LastUpdatedOn = table.Column<DateTime>(nullable: false),
						Title = table.Column<string>(maxLength: 65, nullable: false),
						Urgent = table.Column<bool>(nullable: false),
						VideoEmbedCode = table.Column<int>(nullable: true)
					},
					constraints: table => {
						table.PrimaryKey("PK_News", x => x.Id);
					});
		}

		protected override void Down(MigrationBuilder migrationBuilder) {
			migrationBuilder.DropTable(
					name: "News");
		}
	}
}
