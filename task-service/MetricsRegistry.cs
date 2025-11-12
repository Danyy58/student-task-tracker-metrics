using Prometheus;

namespace task_service
{
    public static class MetricsRegistry
    {
        public static readonly Counter TasksCreatedTotal = Metrics
            .CreateCounter("tasks_created_total", "Количество созданных заданий.");
    }
}