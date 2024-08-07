using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLearningPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AttemptionConfigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentQuizAttempts_AspNetUsers_StudentId",
                table: "StudentQuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                table: "StudentQuizAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentQuizAttempts_AspNetUsers_StudentId",
                table: "StudentQuizAttempts",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                table: "StudentQuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentQuizAttempts_AspNetUsers_StudentId",
                table: "StudentQuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                table: "StudentQuizAttempts");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentQuizAttempts_AspNetUsers_StudentId",
                table: "StudentQuizAttempts",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                table: "StudentQuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
