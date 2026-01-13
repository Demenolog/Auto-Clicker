using AutoClicker.Services.Interfaces;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoClicker.Services.Settings
{
    internal sealed class SettingsService : ISettingsService
    {
        private const int CurrentSettingsVersion = 1;
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly string _settingsPath;

        public SettingsService()
        {
            _settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AutoClicker",
                "settings.json");
        }

        public AppSettings Settings { get; private set; } = new();

        public void Load()
        {
            if (!File.Exists(_settingsPath))
            {
                Settings = new AppSettings();
                return;
            }

            try
            {
                var json = File.ReadAllText(_settingsPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, SerializerOptions);
                Settings = settings ?? new AppSettings();
                Settings.HotKeys ??= new HotKeySettings();
                if (Settings.SettingsVersion < CurrentSettingsVersion)
                {
                    Settings.SettingsVersion = CurrentSettingsVersion;
                }
            }
            catch (IOException)
            {
                Settings = new AppSettings();
            }
            catch (JsonException)
            {
                Settings = new AppSettings();
            }
        }

        public void Save()
        {
            var directory = Path.GetDirectoryName(_settingsPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Settings.SettingsVersion = CurrentSettingsVersion;
            var json = JsonSerializer.Serialize(Settings, SerializerOptions);
            var tempPath = $"{_settingsPath}.tmp";

            try
            {
                File.WriteAllText(tempPath, json);

                if (File.Exists(_settingsPath))
                {
                    File.Replace(tempPath, _settingsPath, null);
                }
                else
                {
                    File.Move(tempPath, _settingsPath, overwrite: true);
                }
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }

        public void Update(Action<AppSettings> updateAction)
        {
            updateAction(Settings);
        }
    }
}
