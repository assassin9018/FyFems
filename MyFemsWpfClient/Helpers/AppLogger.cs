using CommunityToolkit.Mvvm.Messaging;
using MyFems.Clients.Shared.Messages;
using System;
using MSLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace MyFemsWpfClient.Helpers;

internal class AppLogger : IAppLogger
{
    //Экземпляр логгера NLog
    private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

    public AppLogger()
    {
    }

    public void Info(string message)
    {
        _logger.Info(message);
        WeakReferenceMessenger.Default.Send(new ApplicationLog(MSLogLevel.Information, message));
    }

    public void Error(Exception ex)
    {
        var message = $"Source:{ex.Source}, Message:{ex.Message}\nTrace:{ex.StackTrace}";
        WeakReferenceMessenger.Default.Send(new ApplicationLog(MSLogLevel.Error, message));
    }

    public void Error(string message, Exception? ex = null)
    {
        var fullMessage = ex is null ? message : $"Source:{ex.Source}, Message:{ex.Message}, Additional:{message}\nTrace:{ex.StackTrace}";
        _logger.Error(message);
        //Отправляем сообщение в шину
        WeakReferenceMessenger.Default.Send(new ApplicationLog(MSLogLevel.Error, fullMessage, ex));
    }
}

internal interface IAppLogger
{
    void Info(string message);
    void Error(Exception exception);
    void Error(string message, Exception? exception = null);
}