namespace WiredBrainCoffee.MinApi.Models;

public class OrderSystemStatus
{
    public string Status { get; set; }
    public DateTime ScheduledUpdate { get; set; }
    public DateTime UpTime { get; set; }
    public string Version { get; set; }
}
