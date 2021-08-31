using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Data
{
    public class ApplicationDbContext : DbContext
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Todo> Todos { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Status> Statuses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { CategoryId = "work", Name = "Work"},
				new Category { CategoryId = "home", Name = "Home"},
				new Category { CategoryId = "exercise", Name = "Exercise"},
				new Category { CategoryId = "shop", Name = "Shopping"},
				new Category { CategoryId = "contact", Name = "Contact"}
			);

			modelBuilder.Entity<Status>().HasData(
				new Status { StatusId = "open", Name = "Open"},
				new Status { StatusId = "closed", Name = "Completed"}
			);
		}
	}
}
