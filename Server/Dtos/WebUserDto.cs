using Server.DatabaseTables;

namespace Server.ParamClasses;

public class WebUserDto
{
    public string Name { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Interval_Report { get; set; }

    public WebUserDto() { }

    public WebUserDto(User user)
    {
        this.Name = user.Name;
        this.Password = user.Password;
        this.Email = user.Email;
        this.Interval_Report = user.Interval_Report;
    }
}
