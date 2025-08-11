namespace Domain.Common.Interfaces;

public interface IReadOnlyRepository<T> where T: IEnitity
{
    Task<IReadOnlyList<T>> GetAll();
    Task<IReadOnlyList<T>> GetAll(ISpecification<T> specification);
    Task<T?> Get(ISpecification<T> specification);
    Task<int> Count(ISpecification<T>? specification);
}