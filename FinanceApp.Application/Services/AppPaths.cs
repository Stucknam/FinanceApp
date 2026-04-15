using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Application.Services
{
    public class AppPaths
    {
        public string SettingsPath { get; }

        public AppPaths()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var folder = Path.Combine(appData, "FinanceApp");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            SettingsPath = Path.Combine(folder, "settings.json");
        }

    }
}
