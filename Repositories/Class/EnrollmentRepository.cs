using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(ApplicationDbContext context)
        : base(context) { }
}
