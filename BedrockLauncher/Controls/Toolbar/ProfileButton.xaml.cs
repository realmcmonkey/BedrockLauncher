﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BedrockLauncher.Methods;
using System.Windows.Controls.Primitives;
using BedrockLauncher.Classes;
using BedrockLauncher.Controls.Items;
using BedrockLauncher.Pages;
using BedrockLauncher.Pages.Common;

namespace BedrockLauncher.Controls.Toolbar
{
    /// <summary>
    /// Interaction logic for ProfileContextMenu.xaml
    /// </summary>
    public partial class ProfileButton : Grid
    {
        private List<ProfileItem> OtherAccountControls { get; set; } = new List<ProfileItem>();

        public ProfileButton()
        {
            InitializeComponent();
        }

        private void UpdateProfileList()
        {
            foreach (var entry in OtherAccountControls)
            {
                if (ProfileContextMenu.Items.Contains(entry)) ProfileContextMenu.Items.Remove(entry);
            }
            OtherAccountControls.Clear();

            int profileCount = ConfigManager.ProfileList.profiles.Count;

            foreach (var entry in ConfigManager.ProfileList.profiles)
            {
                ProfileItem profile = new ProfileItem(entry, this);
                if (!(profileCount <= 1))
                {
                    OtherAccountControls.Add(profile);
                    ProfileContextMenu.Items.Add(profile);
                }
            }
        }

        private void OpenContextMenu()
        {
            if (OtherAccountControls.Count <= 1)
            {
                RemoveProfileButton.IsEnabled = false;
                OtherAccountsHeader.Visibility = Visibility.Collapsed;
                OtherAccountsSeperator.Visibility = Visibility.Collapsed;
            }
            else
            {
                RemoveProfileButton.IsEnabled = true;
                OtherAccountsHeader.Visibility = Visibility.Visible;
                OtherAccountsSeperator.Visibility = Visibility.Visible;
            }

            ProfileContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Right;
            ProfileContextMenu.PlacementRectangle = new Rect(0, SourceButton.ActualHeight, 0, 0);
            ProfileContextMenu.PlacementTarget = SourceButton;
            ProfileContextMenu.IsOpen = true;
        }

        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateProfileList();
            OpenContextMenu();
        }

        private void ManageProfilesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.LauncherModel.MainThread.NavigateToNewProfilePage();
        }

        private async void RemoveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profile = Properties.LauncherSettings.Default.CurrentProfile;

            var title = this.FindResource("Dialog_DeleteItem_Title") as string;
            var content = this.FindResource("Dialog_DeleteItem_Text") as string;
            var item = this.FindResource("Dialog_Item_Profile_Text") as string;

            var result = await DialogPrompt.ShowDialog_YesNo(title, content, item, profile);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                ConfigManager.RemoveProfile(profile);
            }

        }

        private void ProfileContextMenu_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {

        }

        private void SourceButton_Checked(object sender, RoutedEventArgs e)
        {
            SourceButton.IsChecked = false;
        }
    }
}
