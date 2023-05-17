using Server.DatabaseTables;

namespace Server.Dtos;

public class WebUserNoPass
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Interval_Report { get; set; }

    public WebUserNoPass() { }

    public WebUserNoPass(User user)
    {
        this.Id = user.Id;
        this.Name = user.Name;
        this.Email = user.Email;
        this.Interval_Report = user.Interval_Report;
    }
}
