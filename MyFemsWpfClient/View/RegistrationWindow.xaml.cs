﻿using Microsoft.Toolkit.Mvvm.Messaging;
using MyFems.Clients.Shared.Messages;
using System.Windows;

namespace MyFemsWpfClient.Windows;

/// <summary>
/// Interaction logic for RegistrationWindow.xaml
/// </summary>
public partial class RegistrationWindow : Window
{
    public RegistrationWindow()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<RegistrationWindow, CloseWindowMessage>(this, static (r, m) => r.HandleCloseMessage());
    }

    private void HandleCloseMessage()
        => Close();
}
