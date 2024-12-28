namespace AbstractTaskWorker.Model;

public class AbstractTask
{   
    public Guid Id { get; set; }
    public string Description { get; set; }
    public int TTLInMillisecond { get; set; }
    public string Status { get; set; } //TODO: будет время, поменять на enum
}