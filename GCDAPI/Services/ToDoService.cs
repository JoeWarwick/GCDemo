using GCDRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCDAPI.Services
{
    public class ToDoService : IToDoService
    {
        private ILogger Logger;
        private GCDModel Model;

        public ToDoService(ILogger _logger, GCDModel _model)
        {
            Logger = _logger;
            Model = _model;
        }

        public async Task<ToDoItem> AddToDo(ToDoItem todo)
        {
            todo.Added = DateTime.Now;
            await Model.ToDoItems.AddAsync(todo);
            await Model.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteToDo(int id)
        {
            var todoDel = Model.ToDoItems.FirstOrDefault(t => t.TodoItemId == id);
            if(todoDel != null)
            {
                Model.ToDoItems.Remove(todoDel);
                await Model.SaveChangesAsync();
                return true;
            }
            return false;  
        }

        public IList<ToDoItem> GetToDos()
        {
            return Model.ToDoItems.ToList();
        }

        public async Task<ToDoItem> SetToDo(ToDoItem todo)
        {
            var todoOld = Model.ToDoItems.FirstOrDefault(t => t.TodoItemId == todo.TodoItemId);
            todoOld.Title = todo.Title;
            todoOld.Checked = todo.Checked;
            todoOld.ScheduledBy = todo.ScheduledBy;
            Model.ToDoItems.Update(todoOld);
            await Model.SaveChangesAsync();
            return todoOld;
        }
    }
}