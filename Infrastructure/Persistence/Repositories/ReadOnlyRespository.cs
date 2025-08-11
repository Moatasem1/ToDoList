using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ReadOnlyRespository<T>(AppDbContext context, SpeceficationGenerator<T> generator) : IReadOnlyRepository<T> where T : class, IEnitity
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<int> Count(ISpecification<T>? specification)
    {
        var query = specification is null ? _dbSet.AsQueryable() :  generator.ApplySpecification(specification);

        return await query.CountAsync();
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAll(ISpecification<T> specification)
    {
        var query = generator.ApplySpecification(specification);

        return await query.AsNoTracking().ToListAsync();
    }

 
   public async Task<T?> Get(ISpecification<T> specification)
   {
        var query = generator.ApplySpecification(specification);

        return await query.AsNoTracking().FirstOrDefaultAsync();
   }
}