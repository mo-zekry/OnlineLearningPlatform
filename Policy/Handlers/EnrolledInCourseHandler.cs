using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Repositories;
using OnlineLearningPlatform.Policy.Requirements;

namespace OnlineLearningPlatform.Policy.Handlers;

public class EnrolledInCourseHandler : AuthorizationHandler<EnrolledInCourseRequirement, int> {
    private readonly IUnitOfWork _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public EnrolledInCourseHandler(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) {
        _db = unitOfWork;
        _userManager = userManager;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EnrolledInCourseRequirement requirement,
        int courseId
    ) {
        var userId = _userManager.GetUserId(context.User);
        if (userId == null) return Task.CompletedTask;

        // Check if the user is enrolled in the specified course
        var isEnrolled = _db.Enrollments
            .Get(e => e.StudentId == userId && e.CourseId == courseId)
            .Any();

        if (isEnrolled) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}