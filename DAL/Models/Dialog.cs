﻿using System.ComponentModel.DataAnnotations;

namespace DAL.Models;

#nullable disable
public class Dialog : EntityBase
{
    [Required]
    [MaxLength(64)]
    public string Name { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsDeleted { get; set; }
    public virtual List<Message> Messages { get; set; } = new List<Message>();
    public virtual List<User> Users { get; set; } = new List<User>();
}