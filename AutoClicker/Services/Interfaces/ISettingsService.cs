using AutoClicker.Services.Settings;

namespace AutoClicker.Services.Interfaces
{
    internal interface ISettingsService
    {
        AppSettings Settings { get; }

        void Load();

        void Save();

        void Update(System.Action<AppSettings> updateAction);
    }
}
