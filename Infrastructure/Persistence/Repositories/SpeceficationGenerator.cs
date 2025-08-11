using Domain.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common;

public class SpeceficationGenerator<T>(AppDbContext context) where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        IQueryable<T> query = _dbSet;

        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        foreach (var include in spec.Includes)
            query = query.Include(include);

        return query;
    }
}
