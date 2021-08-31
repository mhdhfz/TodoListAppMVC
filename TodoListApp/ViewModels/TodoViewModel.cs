using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Utilities;

namespace TodoListApp.ViewModels
{
    public class TodoViewModel
    {
		public TodoViewModel()
		{
			CurrentTodo = new Todo();
		}
		public Filters Filters { get; set; }
		public List<Status> Statuses { get; set; }
		public List<Category> Categories { get; set; }
		public Dictionary<string, string> DueFilters { get; set; }
		public List<Todo> Todos { get; set; }
		public Todo CurrentTodo { get; set; }

	}
}
