namespace Infrastructure.API.RightsChecker;

public class UserRightsDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Resource { get; set; }
    public string Action { get; set; }
}