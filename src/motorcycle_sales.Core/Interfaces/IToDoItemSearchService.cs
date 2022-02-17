using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using motorcycle_sales.Core.ProjectAggregate;

namespace motorcycle_sales.Core.Interfaces;

public interface IToDoItemSearchService
{
  Task<Result<ToDoItem>> GetNextIncompleteItemAsync(int projectId);
  Task<Result<List<ToDoItem>>> GetAllIncompleteItemsAsync(int projectId, string searchString);
}
