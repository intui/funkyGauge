using System;

using GalaSoft.MvvmLight;
using System.Collections.Generic;
using WahlDashboard.Models.DataSources;
using Tweetinvi;
using Tweetinvi.Models;
using WahlDashboard.Helpers;
using System.Collections.ObjectModel;

namespace WahlDashboard.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //public List<TwitterStream> TwitterStreams;
        public MainViewModel()
        {
            Authenticate();
            //key1Queue.Duration = new TimeSpan(0, 0, 90);
        }

        internal void Initialize()
        {
            //TwitterStreams = new List<TwitterStream>();
            //TwitterStreams.Add(new TwitterStream("CDU"));
        }

        public string Message { get; private set; }

        private string streamingText;

        /// <summary>
        /// Tweets from the streaming API.
        /// </summary>
        public string StreamingText
        {
            get { return this.streamingText; }
            set
            {
                Set(ref streamingText, value);
            }
        }

        private KeywordQueue[] keyQueues;
        private int keywordFrequency;
        public int KeywordFrequency
        {
            get { return keywordFrequency; }
            set
            {
                Set(ref keywordFrequency, value);
            }
        }
        private ObservableCollection<int> keywordFrequencies;
        public ObservableCollection<int> KeywordFrequencies
        {
            get { return keywordFrequencies; }
            set
            {
                Set(ref keywordFrequencies, value);
            }
        }

        // Max value | todo: add option to set automatically: long term max value
        public int GaugeMax = 100;
        private void Authenticate()
        {
            TwitterConfig.InitApp(); // Initializing credentials -> Auth.SetUserCredentials

            if (Auth.Credentials == null ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerKey) ||
                string.IsNullOrEmpty(Auth.Credentials.ConsumerSecret) ||
                string.IsNullOrEmpty(Auth.Credentials.AccessToken) ||
                string.IsNullOrEmpty(Auth.Credentials.AccessTokenSecret) ||
                Auth.Credentials.AccessToken == "ACCESS_TOKEN")
            {
                Message = "Please enter your credentials in the StreamViewModel.cs file";
            }
            
            else
            {
                var user = User.GetAuthenticatedUser();
                Message = string.Format("Hi '{0}'. Welcome on board with Windows 10 Universal App!", user.Name);
            }
        }

        private string _buffer;

        public void RunFilteredStream()
        {
            var uiDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            //var s = Stream.CreateSampleStream();
            var s = Stream.CreateFilteredStream();
            s.AddTweetLanguageFilter(LanguageFilter.German);
            string[] keywords = "TwitterKeywords".GetLocalized().Split(';');
            keyQueues = new KeywordQueue[keywords.Length];
            KeywordFrequencies = new ObservableCollection<int>();
            //keywordFrequencies = new int[keywords.Length];

            int i = 0;
            foreach (string key in keywords)
            {
                s.AddTrack(key);
                keyQueues[i] = new KeywordQueue();
                keyQueues[i].Duration = new TimeSpan(0, 5, 0);
                KeywordFrequencies.Add(0);
                i++;
            }
            //s.AddTrack("CDU");            //s.AddTrack("Grüne");            //s.AddTrack("Linke");            //s.AddTrack("afd");            //s.AddTrack("fdp");

            s.StallWarnings = true;
            s.StreamStopped += async (sender, args) =>
            {
                var exceptionThatCausedTheStreamToStop = args.Exception;
                var twitterDisconnectMessage = args.DisconnectMessage;
                await uiDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    StreamingText = exceptionThatCausedTheStreamToStop.Message + " " +
                                    exceptionThatCausedTheStreamToStop.StackTrace + "\n" +
                                    twitterDisconnectMessage?.ToString();
                });
                //s.ResumeStream();
                // automatic restart | optional?
                s.StartStreamMatchingAnyCondition();
            };

            s.MatchingTweetReceived += async (o, args) =>
            {
                try
                {
                _buffer += $"{args.Tweet.Text}\r\n";
                    await uiDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        i = 0;
                        foreach (string key in keywords)
                        {
                            if(_buffer.ToLower().Contains(key))
                            {
                                keyQueues[i].Enqueue(DateTime.Now);
                                KeywordFrequencies[i] = keyQueues[i].Count;
                                //KeywordFrequencies[i] = 0; // GaugeMax-1;
                            }
                            i++;
                        }
                        //key1Queue.Enqueue(DateTime.Now);
                        //KeywordFrequency = key1Queue.Count;
                        StreamingText = _buffer;
                    });

                    _buffer = string.Empty;
                }
                catch (Exception ex)
                {
                    // handle it

                }
            };

            s.StartStreamMatchingAnyConditionAsync();
        }

    }
}
