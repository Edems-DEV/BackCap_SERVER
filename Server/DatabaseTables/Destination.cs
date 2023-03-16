﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.DatabaseTables;

public class Destination
{
    [Key]
    public int id { get; set; }

    public int id_Config { get; set; }

    public string DestPath { get; set; }

    [ForeignKey("id_Config")]
    public virtual Config Config { get; set; }
}