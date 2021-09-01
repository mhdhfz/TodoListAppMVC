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
using TodoListApp.Services;
using TodoListApp.Utilities;
using TodoListApp.ViewModels;

namespace TodoListApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly TodoService _todoService;

		public HomeController(TodoService todoService)
		{
			_todoService = todoService;
		}

		public IActionResult Index(string id)
		{
			TodoViewModel model = _todoService.ListTodos(id);
			return View(model);
		}

		[HttpGet]
		public IActionResult Add()
		{
			TodoViewModel model = _todoService.GetTodo();
			return View(model);
		}

		[HttpPost]
		public IActionResult Add(TodoViewModel model)
		{
			if (ModelState.IsValid)
			{
				_todoService.AddTodo(model);
				return RedirectToAction(nameof(Index));
			}

			_todoService.GetCategoriesAndStatuses(model);
			return View(model);
		}

		[HttpPost]
		public IActionResult EditDelete([FromRoute] string id, Todo selected)
		{
			_todoService.EditDeleteTodo(selected);
			return RedirectToAction("Index", "Home", new { ID = id });
		}

		[HttpPost]
		public IActionResult Filter(string[] filter)
		{
			string id = string.Join('-', filter);
			return RedirectToAction("Index", "Home", new { ID = id });
		}



	}
}
