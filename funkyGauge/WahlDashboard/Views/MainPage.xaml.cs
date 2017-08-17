using WahlDashboard.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WahlDashboard.Models.DataSources;
using libPWMdeviceControl;
using System.Threading.Tasks;
using System;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;

namespace WahlDashboard.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }
        DeviceControl pwmDevice = new DeviceControl();
        bool initComplete = false;
        public MainPage()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            //(DataContext as MainViewModel).KeywordFrequencies.CollectionChanged += KeywordFrequencies_CollectionChanged;
            //.KeywordFrequencies.CollectionChanged += KeywordFrequencies_CollectionChanged;
        }

        private void KeywordFrequencies_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //if (initComplete)
            {
                // move servos.
                int i = 0;
                foreach (int val in sender as ObservableCollection<int>)
                {
                    switch (i)
                    {
                        case 0:
                            pwmDevice.S1.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                        case 1:
                            pwmDevice.S2.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                        case 2:
                            pwmDevice.S3.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                        case 3:
                            pwmDevice.S4.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                        case 4:
                            pwmDevice.S5.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                        case 5:
                            pwmDevice.S6.Position_Cmd = Math.Min(Math.Max(0.05f, 1 - (float)val / ViewModel.GaugeMax), 0.95f);
                            break;
                    }
                    i++;
                }
            }
            //else
            {
                {
                    /*
                    var uiDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                    uiDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        for (float j = 0; j < 1; j = j + 0.05f)
                        {
                            pwmDevice.S1.Position_Cmd = j;
                            //Task.Delay(100);
                            pwmDevice.S2.Position_Cmd = j;
                            //Task.Delay(100);
                            pwmDevice.S3.Position_Cmd = j;
                            //Task.Delay(100);
                            pwmDevice.S4.Position_Cmd = j;

                            Task.Delay(100);
                        }
                        initComplete = true;
                    });
                    */
                    //initComplete = true;
                }

            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.RunFilteredStream();
            ViewModel.KeywordFrequencies.CollectionChanged += KeywordFrequencies_CollectionChanged;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize();
            
            pwmDevice.S1 = new Servo(0);
            pwmDevice.S1.MinRange = 0;
            pwmDevice.S1.MaxRange = 1;
            pwmDevice.S2 = new Servo(1);
            pwmDevice.S2.MinRange = 0;
            pwmDevice.S2.MaxRange = 1;
            pwmDevice.S3 = new Servo(2);
            pwmDevice.S3.MinRange = 0;
            pwmDevice.S3.MaxRange = 1;
            pwmDevice.S4 = new Servo(3);
            pwmDevice.S4.MinRange = 0;
            pwmDevice.S4.MaxRange = 1;
            pwmDevice.S5 = new Servo(4);
            pwmDevice.S5.MinRange = 0;
            pwmDevice.S5.MaxRange = 1;
            pwmDevice.S6 = new Servo(5);
            pwmDevice.S6.MinRange = 0;
            pwmDevice.S6.MaxRange = 1;
            int i = 0;
            /*
            ViewModel.TwitterStreams = new System.Collections.Generic.List<TwitterStream>();
            foreach (TwitterStream twitt in ViewModel.TwitterStreams)
            {
                switch (i)
                {
                    case 1:
                        // init servo 0 ...
                        break;
                    default:
                        break;
                }
            }
            */
            var success = pwmDevice.initDevice(0x40); //.GetAwaiter().GetResult();
            /*
            Task.Delay(1000);
            //if(success)
            {
                for (i = 0; i < 80; i+=4)
                {
                    pwmDevice.S1.Position_Cmd = i;
                    //Task.Delay(100);
                    pwmDevice.S2.Position_Cmd = i;
                    //Task.Delay(100);
                    pwmDevice.S3.Position_Cmd = i;
                    //Task.Delay(100);
                    pwmDevice.S4.Position_Cmd = i;
                    //Task.Delay(100);
                    pwmDevice.S5.Position_Cmd = i;
                    //Task.Delay(100);
                    pwmDevice.S6.Position_Cmd = i;
                    Task.Delay(100);
                }
            }
            */
            initComplete = true;
        }
    }
}
