using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace API.Migrations
{
    public partial class InitializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Recording = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    Gender = table.Column<char>(nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    UserGroup_ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_UserGroups_UserGroup_ID",
                        column: x => x.UserGroup_ID,
                        principalTable: "UserGroups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Revoked = table.Column<bool>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    User_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExercises",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Exercise_ID = table.Column<long>(nullable: false),
                    User_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserExercises_Exercises_Exercise_ID",
                        column: x => x.Exercise_ID,
                        principalTable: "Exercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExercises_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPhysios",
                columns: table => new
                {
                    User_ID = table.Column<long>(nullable: false),
                    Physio_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPhysios", x => new { x.User_ID, x.Physio_ID });
                    table.ForeignKey(
                        name: "FK_UserPhysios_Users_Physio_ID",
                        column: x => x.Physio_ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPhysios_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExercisePlannings",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    UserExercise_ID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisePlannings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExercisePlannings_UserExercises_UserExercise_ID",
                        column: x => x.UserExercise_ID,
                        principalTable: "UserExercises",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseSessions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ExercisePlanning_ID = table.Column<long>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseSessions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseSessions_ExercisePlannings_ExercisePlanning_ID",
                        column: x => x.ExercisePlanning_ID,
                        principalTable: "ExercisePlannings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseResults",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Duration = table.Column<int>(nullable: false),
                    ExerciseSession_ID = table.Column<long>(nullable: false),
                    Result = table.Column<byte[]>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseResults", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExerciseResults_ExerciseSessions_ExerciseSession_ID",
                        column: x => x.ExerciseSession_ID,
                        principalTable: "ExerciseSessions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExercisePlannings_UserExercise_ID",
                table: "ExercisePlannings",
                column: "UserExercise_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseResults_ExerciseSession_ID",
                table: "ExerciseResults",
                column: "ExerciseSession_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseSessions_ExercisePlanning_ID",
                table: "ExerciseSessions",
                column: "ExercisePlanning_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_User_ID",
                table: "RefreshTokens",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_Exercise_ID",
                table: "UserExercises",
                column: "Exercise_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_User_ID",
                table: "UserExercises",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPhysios_Physio_ID",
                table: "UserPhysios",
                column: "Physio_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserGroup_ID",
                table: "Users",
                column: "UserGroup_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseResults");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UserPhysios");

            migrationBuilder.DropTable(
                name: "ExerciseSessions");

            migrationBuilder.DropTable(
                name: "ExercisePlannings");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
