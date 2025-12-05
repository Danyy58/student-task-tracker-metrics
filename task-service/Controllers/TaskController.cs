using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_service;
using TaskService.DTO;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController() : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Models.Task?>> CreateTask(TaskRequest request)
        {            
            var number = Random.Shared.Next(0, 10);
            if (number >= 7)
                return BadRequest("Не удалось создать задачу");

            MetricsRegistry.TasksCreatedTotal.Inc();
            return Ok("Задача успешно создана");
        }
    }
}
