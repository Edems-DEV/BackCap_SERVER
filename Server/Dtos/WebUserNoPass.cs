namespace Server.Dtos;

public class WebUserNoPass
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Interval_Report { get; set; }

    public WebUserNoPass(int id, string name, string email, string interval_Report)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Interval_Report = interval_Report;
    }
}
