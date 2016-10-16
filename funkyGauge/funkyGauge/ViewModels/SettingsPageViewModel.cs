using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.UI.Xaml;
using funkyGauge.Models;
using System.Collections.ObjectModel;

namespace funkyGauge.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        Services.SettingsServices.SettingsService _settings;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = Services.SettingsServices.SettingsService.Instance;
            }
        }

        public ObservableCollection<PegelOnlineWSVAdapter> GaugeAdapters
        {
            get
            {
                if (localSettings.Values["adapters"] == null)
                {
                    return new ObservableCollection<PegelOnlineWSVAdapter>();
                }
                else
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<PegelOnlineWSVAdapter>>(localSettings.Values["adapters"].ToString());
                    //return (List<IAdapter>)localSettings.Values["adapters"];
                }
            }
            set
            {
                localSettings.Values["adapters"] = JsonConvert.SerializeObject(value);
            }
        }
        DelegateCommand _AddAdapter;
        public DelegateCommand AddAdapter
            => _AddAdapter ?? (_AddAdapter = new DelegateCommand( () =>
        {
            PegelOnlineWSVAdapter myPegel = new PegelOnlineWSVAdapter();
            myPegel.UpdateFrequency = 60;
            myPegel.Configuration = "super";
            //GaugeAdapters.Add(myPegel);
            ObservableCollection<PegelOnlineWSVAdapter> myAdapter;
            if (localSettings.Values["adapters"] != null)
            {
                myAdapter = JsonConvert.DeserializeObject<ObservableCollection<PegelOnlineWSVAdapter>>(localSettings.Values["adapters"].ToString());
            }
            else
            {
                myAdapter = new ObservableCollection<PegelOnlineWSVAdapter>();
            }
            myAdapter.Add(myPegel);
            GaugeAdapters = myAdapter;
        } ));

        public bool UseShellBackButton
        {
            get { return _settings.UseShellBackButton; }
            set { _settings.UseShellBackButton = value; base.RaisePropertyChanged(); }
        }

        public bool UseLightThemeButton
        {
            get { return _settings.AppTheme.Equals(ApplicationTheme.Light); }
            set { _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged(); }
        }

        private string _BusyText = "Please wait...";
        public string BusyText
        {
            get { return _BusyText; }
            set
            {
                Set(ref _BusyText, value);
                _ShowBusyCommand.RaiseCanExecuteChanged();
            }
        }

        DelegateCommand _ShowBusyCommand;
        public DelegateCommand ShowBusyCommand
            => _ShowBusyCommand ?? (_ShowBusyCommand = new DelegateCommand(async () =>
            {
                Views.Busy.SetBusy(true, _BusyText);
                await Task.Delay(5000);
                Views.Busy.SetBusy(false);
            }, () => !string.IsNullOrEmpty(BusyText)));
    }

    public class AboutPartViewModel : ViewModelBase
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }
}

