﻿using System;
using System.IO;
using System.Windows.Controls;
using System.Windows;


namespace BedrockLauncherSetup.Pages
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class InstallTypePage : Page
    {
        private bool IsInit = false;
        private bool IsFullInit = false;
        public InstallTypePage()
        {
            InitializeComponent();
        }

        private void RadioButton_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (IsInit && IsFullInit)
            {
                BedrockLauncherSetup.MainWindow.Installer.IsBeta = BetaRadioButton.IsChecked.Value;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BetaRadioButton.IsChecked = BedrockLauncherSetup.MainWindow.Installer.IsBeta;
            IsFullInit = true;
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            IsInit = true;
        }
    }
}
