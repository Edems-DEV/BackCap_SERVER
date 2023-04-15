namespace Server.Dtos;

public class WebGroupDto
{
    public int Id { get; set; } 

    public string Name { get; set; }

    public string? Description { get; set; }

    public WebGroupDto(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public WebGroupDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
