using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Class;

namespace OnlineLearningPlatform.Repositories.Interface;

public class ModuleRepository : GenericRepository<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context)
        : base(context) { }
}
