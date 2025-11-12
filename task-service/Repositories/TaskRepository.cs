
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskService.Data;

namespace TaskService.Repositories
{
    public class TaskRepository(AppDbContext context) : ITaskRepository
    {
        public async Task<int?> AddTask(Models.Task task)
        {
            try
            {
                context.Add(task);
                await context.SaveChangesAsync();
                return task.ID;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Models.Task>> GetAllTasks(int authorId)
        {
            var tasks = await context.Task.Where(t => t.AuthorID == authorId).ToListAsync();

            return tasks;
        }

        public async Task<Models.Task?> GetTask(int taskId, int authorId)
        {
            var task = await context.Task.FirstOrDefaultAsync(t => t.ID == taskId && t.AuthorID == authorId);
            return task;
        }

        public async Task Save(Models.Task result)
        {
            Models.Task task = (await context.Task.FindAsync(result.ID))!;
            task = result;
            await context.SaveChangesAsync();
        }

        public async Task Delete(Models.Task result)
        {
            context.Task.Remove(result);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserTasksAsync(int userId)
        {
            var tasks = await context.Task.Where(t => t.AuthorID == userId).ToListAsync();
            context.Task.RemoveRange(tasks);
            await context.SaveChangesAsync();
        }
    }
}
