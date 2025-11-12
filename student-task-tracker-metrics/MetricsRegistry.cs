using Prometheus;

namespace task_service
{
    public static class MetricsRegistry
    {
        public static readonly Counter RegisteredTotal = Metrics
            .CreateCounter("registered_total", "Количество созданных учетных записей.");
    }
}
