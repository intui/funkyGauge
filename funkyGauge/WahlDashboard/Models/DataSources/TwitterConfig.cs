﻿using Tweetinvi;

namespace WahlDashboard.Models.DataSources
{
    public static class TwitterConfig
    {
        public static void InitApp()
        {
            Auth.SetUserCredentials("A","B","c","d");
        }
    }
}
