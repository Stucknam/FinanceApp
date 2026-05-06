
using FinanceApp.Domain.Enums;
using Npgsql.Replication.PgOutput;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FinanceApp.ConsoleUI.Services 
{
    public class SettingsService : ISettingsService
    {
        private readonly string _filePath;

        private Guid? _defaultAccountId;
        private List<Guid> _hiddenAccounts = new();
        private ApplicationTheme _theme = ApplicationTheme.Light;
        private static readonly SemaphoreSlim semLock = new(1, 1);

        public Guid? DefaultAccountId
        {
            get => _defaultAccountId;
            set
            {
                _defaultAccountId = value;
                _ = SaveAsync();
            }
        }

        public List<Guid> HiddenAccounts
        {
            get => _hiddenAccounts;
            set
            {
                _hiddenAccounts = value ?? new List<Guid>();
                _ = SaveAsync();
            }
        }

        public ApplicationTheme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                _ = SaveAsync();
            }
        }

        public SettingsService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task LoadAsync()
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);

                var loaded = JsonSerializer.Deserialize<SettingsDto>(json);

                if (loaded != null)
                {
                    _defaultAccountId = loaded.DefaultAccountId;
                    _hiddenAccounts = loaded.HiddenAccounts ?? new List<Guid>();
                    _theme = loaded.Theme;
                }
            }
            catch
            {
                // Если JSON повреждён — просто игнорируем и оставляем настройки по умолчанию
                _defaultAccountId = null;
                _hiddenAccounts = new List<Guid>();
                _theme = ApplicationTheme.Light;

            }
        }

        public async Task SaveAsync()
        {

            SettingsDto dto = new SettingsDto
            {
                DefaultAccountId = DefaultAccountId,
                HiddenAccounts = HiddenAccounts ?? [],
                Theme = Theme
            };

            var json = JsonSerializer.Serialize<SettingsDto>(dto, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await semLock.WaitAsync();
            try
            {
                
                await File.WriteAllTextAsync(_filePath, json);
            } finally { semLock.Release(); }
            
        }

        public Guid? GetDefaultAccountId()
        {
            return DefaultAccountId;
        }

        public void SetDefaultAccountId(Guid accountId)
        {
            DefaultAccountId = accountId;
        }

        public void HideAccount(Guid accountId)
        {
            HiddenAccounts.Add(accountId);
        }

        public void ShowAccount(Guid accountId)
        {
            HiddenAccounts.Remove(accountId);
        }
    }
}
