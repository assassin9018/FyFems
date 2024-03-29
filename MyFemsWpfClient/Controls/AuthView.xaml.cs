﻿using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using MyFems.Clients.Shared.Messages;
using MyFemsWpfClient.Windows;
using System.Windows.Controls;

namespace MyFemsWpfClient.Controls;

/// <summary>
/// Interaction logic for AuthPage.xaml
/// </summary>
public partial class AuthView : UserControl
{
    public AuthView()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<AuthView, ShowRegistrationViewMessage>(this, static (r, m) => r.HandleShowMessage());
    }

    private void HandleShowMessage()
    {
        App.ServiceProvider.GetService<RegistrationWindow>()?.ShowDialog();
    }
}
