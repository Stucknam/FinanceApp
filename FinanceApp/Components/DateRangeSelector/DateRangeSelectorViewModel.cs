using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Components.DateRangeSelector
{
    public partial class DateRangeSelectorViewModel:ObservableObject
    {
        [ObservableProperty]
        private DateTime start;
        [ObservableProperty]
        private DateTime end;
    }
}
