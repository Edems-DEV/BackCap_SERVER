namespace Server.Dtos;

public class WebOthersDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public WebOthersDto(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
}
