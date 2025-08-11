namespace Domain.Common.Interfaces;

public interface IUnitOfWork
{
   Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
