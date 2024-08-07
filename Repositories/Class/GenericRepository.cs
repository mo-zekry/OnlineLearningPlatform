using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Context;
using OnlineLearningPlatform.Repositories.Interface;

namespace OnlineLearningPlatform.Repositories.Class;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class {
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context) {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <summary>
    ///     Retrieves a collection of entities from the database based on the specified filter, order, and include properties.
    /// </summary>
    /// <param name="filter">An optional expression to filter the entities.</param>
    /// <param name="orderBy">An optional function to order the entities.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include.</param>
    /// <returns>A collection of entities that match the specified filter and order.</returns>
    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = ""
    ) {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null) query = query.Where(filter);

        foreach (
            var includeProperty in includeProperties.Split(
                new[] { ',' },
                StringSplitOptions.RemoveEmptyEntries
            )
        )
            query = query.Include(includeProperty);

        if (orderBy != null)
            return orderBy(query).ToList();
        return query.ToList();
    }

    /// <summary>
    ///     Retrieves an entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity with the specified identifier, or null if it does not exist.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the entity with the specified identifier does not exist.</exception>
    public virtual TEntity GetByID(object id) {
        return _dbSet.Find(id) ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     Inserts a new entity into the database.
    /// </summary>
    /// <param name="entity">The entity to be inserted.</param>
    public virtual void Insert(TEntity entity) {
        _dbSet.Add(entity);
    }

    /// <summary>
    ///     Deletes an entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to be deleted.</param>
    /// <exception cref="InvalidOperationException">Thrown if the entity with the specified identifier does not exist.</exception>
    public virtual void Delete(object id) {
        var entityToDelete = _dbSet.Find(id) ?? throw new InvalidOperationException();
        Delete(entityToDelete);
    }

    /// <summary>
    ///     Deletes an entity from the database.
    /// </summary>
    /// <param name="entityToDelete">The entity to be deleted.</param>
    public virtual void Delete(TEntity entityToDelete) {
        // Check if the entity is in the detached state
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
            // Attach the entity to the context
            _dbSet.Attach(entityToDelete);

        // Remove the entity from the set
        _dbSet.Remove(entityToDelete);
    }

    /// <summary>
    ///     Updates the specified entity in the database.
    /// </summary>
    /// <param name="entityToUpdate">The entity to be updated.</param>
    /// <exception cref="System.ArgumentNullException">Thrown if the entity is null.</exception>
    public virtual void Update(TEntity entityToUpdate) {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}