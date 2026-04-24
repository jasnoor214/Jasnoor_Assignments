using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BEC.Migrations
{
    /// <inheritdoc />
    public partial class identityaddedhkdmddjdkd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Resorts_CourseId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resorts",
                table: "Resorts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "payments");

            migrationBuilder.RenameTable(
                name: "Resorts",
                newName: "courses");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "enquiries");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId1",
                table: "payments",
                newName: "IX_payments_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CourseId",
                table: "payments",
                newName: "IX_payments_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId1",
                table: "enquiries",
                newName: "IX_enquiries_UserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payments",
                table: "payments",
                column: "PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_enquiries",
                table: "enquiries",
                column: "EnquiryId");

            migrationBuilder.AddForeignKey(
                name: "FK_enquiries_AspNetUsers_UserId1",
                table: "enquiries",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_AspNetUsers_UserId1",
                table: "payments",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_courses_CourseId",
                table: "payments",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_enquiries_AspNetUsers_UserId1",
                table: "enquiries");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_AspNetUsers_UserId1",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_payments_courses_CourseId",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payments",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_enquiries",
                table: "enquiries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.RenameTable(
                name: "payments",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "enquiries",
                newName: "Bookings");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Resorts");

            migrationBuilder.RenameIndex(
                name: "IX_payments_UserId1",
                table: "Reviews",
                newName: "IX_Reviews_UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_payments_CourseId",
                table: "Reviews",
                newName: "IX_Reviews_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_enquiries_UserId1",
                table: "Bookings",
                newName: "IX_Bookings_UserId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "PaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "EnquiryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resorts",
                table: "Resorts",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId1",
                table: "Bookings",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Resorts_CourseId",
                table: "Reviews",
                column: "CourseId",
                principalTable: "Resorts",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
