namespace Server.ParamClasses;

public class JobAdminDto
{
    public int Id_Machine { get; set; }

    public int? Id_Group { get; set; }

    public int Id_Config { get; set; }
    
    public Int16 Status { get; set; }

    public DateTime Time_Schedule { get; set; }

    public int? Bytes { get; set; }
}
