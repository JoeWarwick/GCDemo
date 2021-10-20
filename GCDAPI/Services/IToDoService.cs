using GCDRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GCDAPI.Services
{
    public interface IToDoService
    {
        IList<ToDoItem> GetToDos();
        Task<ToDoItem> SetToDo(ToDoItem todo);
        Task<ToDoItem> AddToDo(ToDoItem todo);
        Task<bool> DeleteToDo(int id);
    }
}
