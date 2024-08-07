using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Models;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class ModuleRepository : GenericRepository<Module>, IModuleRepository {
    public ModuleRepository(ApplicationDbContext context)
        : base(context) { }
}