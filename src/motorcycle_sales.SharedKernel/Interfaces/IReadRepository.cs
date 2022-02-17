using Ardalis.Specification;

namespace motorcycle_sales.SharedKernel.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
