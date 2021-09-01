using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListApp.Utilities;
using TodoListApp.ViewModels;

namespace TodoListApp.Services
{
    public class TodoService
    {
		private readonly ApplicationDbContext _context;

		public TodoService(ApplicationDbContext context)
		{
			_context = context;
		}
		public TodoViewModel ListTodos(string id)
		{
			var model = new TodoViewModel
			{
				Filters = new Filters(id),
				Categories = _context.Categories.ToList(),
				Statuses = _context.Statuses.ToList(),
				DueFilters = Filters.DueFilterValues
			};

			IQueryable<Todo> query = _context.Todos.Include(c => c.Category).Include(s => s.Status);

			if (model.Filters.HasCategory)
				query = query.Where(t => t.CategoryId == model.Filters.CategoryId);

			if (model.Filters.HasStatus)
				query = query.Where(t => t.StatusId == model.Filters.StatusId);

			if (model.Filters.HasDue)
			{
				var today = DateTime.Today;
				if (model.Filters.IsPast)
				{
					query = query.Where(t => t.DueDate < today);
				}
				else if (model.Filters.IsFuture)
				{
					query = query.Where(t => t.DueDate > today);
				}
				else if (model.Filters.IsToday)
				{
					query = query.Where(t => t.DueDate == today);
				}
			}

			var todos = query.OrderBy(t => t.DueDate).ToList();
			model.Todos = todos;
			return model;
		}

		public TodoViewModel GetTodo()
		{
			return new TodoViewModel
			{
				Categories = _context.Categories.ToList(),
				Statuses = _context.Statuses.ToList()
			};
		}

		public void AddTodo(TodoViewModel model)
		{
			_context.Todos.Add(model.CurrentTodo);
			_context.SaveChanges();
		}

		public void GetCategoriesAndStatuses(TodoViewModel model)
		{
			model.Categories = _context.Categories.ToList();
			model.Statuses = _context.Statuses.ToList();
		}

		public Todo EditDeleteTodo(Todo selected)
		{
			if (selected.StatusId == null)
			{
				_context.Todos.Remove(selected);
			}
			else
			{
				string newStatusId = selected.StatusId;
				selected = _context.Todos.Find(selected.Id);
				selected.StatusId = newStatusId;
				_context.Todos.Update(selected);
			}
			_context.SaveChanges();
			return selected;
		}
	}
}
