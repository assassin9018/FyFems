﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientLocalDAL.Models;

#nullable disable
public class User : EntityBase
{
    [Required]
    [MaxLength(32)]
    public string Name { get; set; }
    [Required]
    [MaxLength(32)]
    public string Surname { get; set; }
    [Required]
    [MaxLength(48)]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string Phone { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateOnly BirthDate { get; set; }
    public string FullName { get; }
    public int Age { get; }
    [Required]
    public string PasswordHash { get; set; }
    public virtual List<Dialog> UserDialogs { get; set; } = new();
    [InverseProperty(nameof(Contact.User))]
    public virtual List<Contact> Contacts { get; set; } = new();
}