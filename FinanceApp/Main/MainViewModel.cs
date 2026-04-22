using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FinanceApp.Pages.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Main
{
    public partial class MainViewModel: ObservableObject
    { 
        [ObservableProperty]
        private object currentPage;

        [ObservableProperty]
        private object? overlay;

        public MainViewModel(DashboardViewModel dashboard)
        {
            CurrentPage = dashboard;
        }

        [RelayCommand]
        private void ShowOverlay(object vm) => Overlay = vm;

        [RelayCommand]
        private void CloseOverlay() => Overlay = null;

    }
}
