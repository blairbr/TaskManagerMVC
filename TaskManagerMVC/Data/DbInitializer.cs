using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TaskContext context)
        {
            context.Database.EnsureCreated();

            if (context.Tasks.Any())
            {
                return;
            }

            // added some tasks to seed db
            var tasks = new Models.Task[]
            {
            new Models.Task{Description="Do Laundry",CompletionStatus=true,DueDate=DateTime.Parse("2020-09-01")},
            new Models.Task{Description="Finish Tech Challenge",CompletionStatus=true,DueDate=DateTime.Parse("2020-03-09")},
            new Models.Task{Description="Pack for Vacation",CompletionStatus=true,DueDate=DateTime.Parse("2020-02-26")}
            };
            foreach (var task in tasks)
            {
                context.Tasks.Add(task);
            }
            context.SaveChanges();
        }
    }
}
