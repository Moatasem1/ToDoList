using System.Linq.Expressions;

namespace Domain.Common.Interfaces;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<List<T>> GetAll();
    Task<List<T>> GetAll(ISpecification<T> specification);
    Task<T?> Get(ISpecification<T> specification);
    Task Add(T entity);
    Task AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
    Task<int> Count(ISpecification<T> specification);
}