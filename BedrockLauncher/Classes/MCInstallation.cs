﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using BedrockLauncher.Methods;
using BL_Core.Components;

namespace BedrockLauncher.Classes
{
    public class MCInstallation : NotifyPropertyChangedBase
    {
        public string DisplayName { get; set; }
        public string VersionUUID { get; set; }
        public string IconPath { get; set; }
        public bool IsCustomIcon { get; set; } = false;
        public string DirectoryName { get; set; }
        public bool ReadOnly { get; set; }

        public bool UseLatestVersion { get; set; }
        public bool UseLatestBeta { get; set; }


        [JsonIgnore]
        public string IconPath_Full
        {
            get
            {
                if (IsCustomIcon) return System.IO.Path.Combine(Filepaths.GetCacheFolderPath(), IconPath);
                else return Filepaths.PrefabedIconRootPath + IconPath;
            }
            set
            {
                IconPath = System.IO.Path.GetFileName(value);
            }
        }
        [JsonIgnore]
        public string DisplayName_Full
        {
            get
            {
                if (VersionUUID == "latest_release" && ReadOnly) return Application.Current.FindResource("VersionEntries_LatestRelease").ToString();
                else if (VersionUUID == "latest_beta" && ReadOnly) return Application.Current.FindResource("VersionEntries_LatestSnapshot").ToString();
                else return DisplayName;
            }
        }
        [JsonIgnore]
        public string DirectoryName_Full
        {
            get
            {
                if (string.IsNullOrEmpty(DirectoryName))
                {
                    char[] invalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
                    string result = new string(DisplayName.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
                    return result;
                }
                else return DirectoryName;
            }
        }
        

        [JsonIgnore]
        public MCVersion Version
        {
            get
            {

                if (UseLatestVersion)
                {
                    var latest_beta = ConfigManager.Versions.ToList().FirstOrDefault(x => x.IsBeta == true);
                    var latest_release = ConfigManager.Versions.ToList().FirstOrDefault(x => x.IsBeta == false);

                    if (UseLatestBeta && latest_beta != null) return latest_beta;
                    else if (latest_release != null) return latest_release;
                }
                else if (ConfigManager.Versions.ToList().Exists(x => x.UUID == VersionUUID))
                {
                    return ConfigManager.Versions.ToList().Where(x => x.UUID == VersionUUID).FirstOrDefault();
                }
                return null;
            }
        }

        [JsonIgnore]
        public bool IsBeta
        {
            get
            {
                if (UseLatestVersion && UseLatestBeta) return true;
                else return Version?.IsBeta ?? false;
            }
        }

        [JsonIgnore]
        public string VersionName
        {
            get
            {
                string version = Version?.Name ?? string.Empty;
                if (UseLatestBeta) return Application.Current.FindResource("VersionEntries_LatestSnapshot").ToString();
                else if (UseLatestVersion) return Application.Current.FindResource("VersionEntries_LatestRelease").ToString();
                else return version;
            }
        }


        public void Update()
        {
            OnPropertyChanged(nameof(DisplayName_Full));
            OnPropertyChanged(nameof(DirectoryName_Full));
            OnPropertyChanged(nameof(IconPath_Full));
            OnPropertyChanged(nameof(VersionName));
        }

    }
}
