using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context)
        : base(context) { }
}
