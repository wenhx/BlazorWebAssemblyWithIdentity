using BlazorWebAssemblyWithIdentity.Shared;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace BlazorWebAssemblyWithIdentity.Server.Migrations
{
    public partial class AddDefaultRoles : Migration
    {
        static readonly string s_AspNetRolesTableName = "AspNetRoles";
        static readonly string s_AspNetRolesTableKeyName = "Id";
        static readonly Guid s_AdminRoleId = new Guid("B3317BB4-ADC6-444D-A288-75610EC93F04");
        static readonly Guid s_UserRoleId = new Guid("BCFC3268-60A5-4FCC-B9B7-E258D9D1B481");

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            InsertRole(migrationBuilder, s_AdminRoleId, Constants.RoleNames.Admin);
            InsertRole(migrationBuilder, s_UserRoleId, Constants.RoleNames.User);
        }

        private static void InsertRole(MigrationBuilder migrationBuilder, Guid roleId, string roleName)
        {
            migrationBuilder.InsertData(
                            table: s_AspNetRolesTableName,
                            columns: new[] { s_AspNetRolesTableKeyName, "Name", "NormalizedName", "ConcurrencyStamp" },
                            values: new object[] { roleId, roleName, roleName.ToUpper(), Guid.NewGuid().ToString() });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            DeleteRole(migrationBuilder, s_AdminRoleId);
            DeleteRole(migrationBuilder, s_UserRoleId);
        }

        private static void DeleteRole(MigrationBuilder migrationBuilder, Guid roleId)
        {
            migrationBuilder.DeleteData(
                            table: s_AspNetRolesTableName,
                            keyColumn: s_AspNetRolesTableKeyName,
                            keyValue: roleId);
        }
    }
}