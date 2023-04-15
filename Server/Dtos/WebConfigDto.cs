using Server.DatabaseTables;

namespace Server.Dtos;

public class WebConfigDto
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public int Type { get; set; }

    public bool IsCompressed { get; set; }

    public int PackageSize { get; set; }

    public int Retencion { get; set; }

    public string Interval { get; set; }

    public DateTime? EndOfInterval { get; set; }

    public List<Sources> Sources { get; set; }

    public List<Destination> Destinations { get; set; }

    public Machine Machine { get; set; }

    public Groups Group { get; set; }
}
