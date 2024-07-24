using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Context.Identity;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class StudentRepository : GenericRepository<ApplicationUser>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context)
        : base(context) { }
}
