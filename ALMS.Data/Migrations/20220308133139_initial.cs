using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ALMS.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityActivityLog",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    EntityName = table.Column<string>(nullable: false),
                    EntityTypeName = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: false),
                    EntityObject = table.Column<string>(nullable: true),
                    CrudType = table.Column<int>(nullable: false),
                    Changes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityActivityLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    PasswordHashCode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "App",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsApprovalActive = table.Column<bool>(nullable: false),
                    ApiKey = table.Column<string>(nullable: false),
                    ApiSecretKey = table.Column<string>(nullable: false),
                    PasswordHashCode = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App", x => x.Id);
                    table.ForeignKey(
                        name: "FK_App_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppLimit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    AppId = table.Column<int>(nullable: false),
                    LimitName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLimit_App_AppId",
                        column: x => x.AppId,
                        principalTable: "App",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppLimit_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    AppId = table.Column<int>(nullable: false),
                    Product = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProduct_App_AppId",
                        column: x => x.AppId,
                        principalTable: "App",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProduct_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Company_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "License",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    AppId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    LicenseNo = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LicenseProducts = table.Column<string>(nullable: true),
                    LicensePeriodType = table.Column<int>(nullable: false),
                    LicensePeriod = table.Column<int>(nullable: false),
                    SessionLimit = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    RegisteredDate = table.Column<DateTime>(nullable: true),
                    AFUid = table.Column<string>(nullable: true),
                    AFStatus = table.Column<int>(nullable: false),
                    AFMessage = table.Column<string>(nullable: true),
                    RegisteredIP = table.Column<string>(nullable: true),
                    RegisteredMechineName = table.Column<string>(nullable: true),
                    RegisteredUserAgent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.Id);
                    table.ForeignKey(
                        name: "FK_License_App_AppId",
                        column: x => x.AppId,
                        principalTable: "App",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_License_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_License_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseLimit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    LicenseId = table.Column<int>(nullable: false),
                    AppLimitId = table.Column<int>(nullable: false),
                    Limit = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseLimit_AppLimit_AppLimitId",
                        column: x => x.AppLimitId,
                        principalTable: "AppLimit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseLimit_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseLimit_License_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "License",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenseProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    LicenseId = table.Column<int>(nullable: false),
                    AppProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenseProduct_AppProduct_AppProductId",
                        column: x => x.AppProductId,
                        principalTable: "AppProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseProduct_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseProduct_License_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "License",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AppId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LicenseId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    SessionUId = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    LastActivityDate = table.Column<DateTime>(nullable: false),
                    IP = table.Column<string>(nullable: false),
                    MechineName = table.Column<string>(nullable: false),
                    UserAgent = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_App_AppId",
                        column: x => x.AppId,
                        principalTable: "App",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Session_License_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "License",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreateDate", "Email", "IsActive", "Name", "Password", "PasswordHashCode", "Role", "Surname" },
                values: new object[] { 1, new DateTime(2022, 3, 8, 16, 31, 39, 155, DateTimeKind.Local).AddTicks(2180), "taner.selek@elsabilisim.com", true, "system", "RH0mFbWRdT+sGr8cUfc7JZ30I/qNuWP//K4SGcgEAX8=", "afewg32ggh_235", 9, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_App_ApiKey",
                table: "App",
                column: "ApiKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_App_CreateUserId",
                table: "App",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_App_Name",
                table: "App",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppLimit_CreateUserId",
                table: "AppLimit",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLimit_AppId_LimitName",
                table: "AppLimit",
                columns: new[] { "AppId", "LimitName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProduct_CreateUserId",
                table: "AppProduct",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProduct_AppId_Product",
                table: "AppProduct",
                columns: new[] { "AppId", "Product" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_CreateUserId",
                table: "Company",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_OrganizationId_Name",
                table: "Company",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_License_AppId",
                table: "License",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_License_CompanyId",
                table: "License",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_License_CreateUserId",
                table: "License",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_License_LicenseNo",
                table: "License",
                column: "LicenseNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseLimit_AppLimitId",
                table: "LicenseLimit",
                column: "AppLimitId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseLimit_CreateUserId",
                table: "LicenseLimit",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseLimit_LicenseId_AppLimitId",
                table: "LicenseLimit",
                columns: new[] { "LicenseId", "AppLimitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_AppProductId",
                table: "LicenseProduct",
                column: "AppProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_CreateUserId",
                table: "LicenseProduct",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseProduct_LicenseId_AppProductId",
                table: "LicenseProduct",
                columns: new[] { "LicenseId", "AppProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organization_CreateUserId",
                table: "Organization",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organization_Name",
                table: "Organization",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_AppId",
                table: "Session",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_LicenseId",
                table: "Session",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityActivityLog");

            migrationBuilder.DropTable(
                name: "LicenseLimit");

            migrationBuilder.DropTable(
                name: "LicenseProduct");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "AppLimit");

            migrationBuilder.DropTable(
                name: "AppProduct");

            migrationBuilder.DropTable(
                name: "License");

            migrationBuilder.DropTable(
                name: "App");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
