using Ardalis.Specification;

namespace motorcycle_sales.SharedKernel.Interfaces;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
