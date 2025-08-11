using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistence.Repositories;

public class Repository<T>(AppDbContext context, SpeceficationGenerator<T> generator) : IRepository<T> where T : class,IAggregateRoot
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    public async Task Add(T entity)
    {
       await _dbSet.AddAsync(entity);
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public Task<int> Count(ISpecification<T> specification)
    {
        return generator.ApplySpecification(specification).CountAsync();
    }

    public async Task<T?> Get(ISpecification<T> specification)
    {
        var query = generator.ApplySpecification(specification);

        return await query.FirstOrDefaultAsync();
    }

   public async Task<List<T>> GetAll(ISpecification<T> specification)
    {
        var query = generator.ApplySpecification(specification);

        return await query.ToListAsync();
    }

    public async Task<List<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
      _dbSet.RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

   
}
