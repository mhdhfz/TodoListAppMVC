using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListApp.Utilities;
using TodoListApp.ViewModels;

namespace TodoListApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HomeController(ApplicationDbContext context) => _context = context;

		public IActionResult Index(string id)
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

			return View(model);
		}


		[HttpGet]
		public IActionResult Add()
		{
			var model = new TodoViewModel
			{
				Categories = _context.Categories.ToList(),
				Statuses = _context.Statuses.ToList()
			};

			return View(model);
		}


		[HttpPost]
		public IActionResult Add(TodoViewModel model)
		{
			if (ModelState.IsValid)
			{
				_context.Todos.Add(model.CurrentTodo);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}

			model.Categories = _context.Categories.ToList();
			model.Statuses = _context.Statuses.ToList();
			return View(model);
		}


		[HttpPost]
		public IActionResult EditDelete([FromRoute] string id, Todo selected)
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

			return RedirectToAction("Index","Home", new { ID = id });
		}


		[HttpPost]
		public IActionResult Filter(string[] filter)
		{
			string id = string.Join('-', filter);
			return RedirectToAction("Index", "Home", new { ID = id });
		}



	}
}
