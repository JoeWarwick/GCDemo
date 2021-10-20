using Microsoft.EntityFrameworkCore;
using System;

namespace GCDRepository
{
    public class GCDModel: DbContext
    {
        public GCDModel(DbContextOptions options) : base(options) {}

        public DbSet<ToDoItem> ToDoItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoItem>()
            .Property(f => f.TodoItemId)
            .ValueGeneratedOnAdd();
        }
    }

    public class ToDoItem
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public bool Checked { get; set; }
        public DateTime ScheduledBy { get; set; }
		public DateTime Added { get; set; }		
    }
}
