namespace AbstractTaskService.DAL.Entities;

public class AbstractTask
{   
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int TTLInMilliseconds { get; set; }
    public string Status { get; set; }
}