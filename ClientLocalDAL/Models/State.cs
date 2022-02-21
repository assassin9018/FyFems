using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLocalDAL.Models;

#nullable disable
public class State : EntityBase
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool SavePassword { get; set; }
    public string RefrashToken { get; set; }
    public Guid Avatar { get; set; }
    public AppTheme Theme { get; set; }
}

public enum AppTheme
{
    System = 0,
    Dark = 1,
    Light = 2,
}
