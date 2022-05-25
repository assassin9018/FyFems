using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFems.Clients.Shared.Services;
public interface IPasswordProvider
{
    string GetPassword();
}
