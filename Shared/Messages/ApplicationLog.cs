using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFems.Clients.Shared.Messages;
public class ApplicationLog
{
    public ApplicationLog(LogLevel information, string message)
    {
        Information = information;
        Message = message;
    }

    public ApplicationLog(LogLevel information, string message, Exception? ex) : this(information, message)
    {
        Ex = ex;
    }

    public LogLevel Information { get; }
    public string Message { get; }
    public Exception? Ex { get; }
}
