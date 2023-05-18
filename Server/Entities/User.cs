using Server.ParamClasses;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }

    public string Interval_Report { get; set; }

    public User() { }

    public User(WebUserDto user)
    {
        this.Name = user.Name;
        this.Password = user.Password;
        this.Email = user.Email;
        this.Interval_Report = user.Interval_Report;
    }

    public void UpdateUser(User user)
    {
        this.Id = user.Id;
        this.Name = user.Name;

        if (user.Password != "")
            this.Password = user.Password;

        this.Email = user.Email;
        this.Interval_Report = user.Interval_Report;
    }
}
