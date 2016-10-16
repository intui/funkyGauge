using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funkyGauge.Models
{
    public class PegelOnlineWSVAdapter : IAdapter
    {
        int updateFrequency;
        string configuration;
        public string Title
        {
            get
            {
                return "Pegel Online WSV";
            }
        }
        public string Description
        {
            get
            {
                return "Pegelstände von deutschen Gewässern";
            }
        }
        public Uri Endpoint
        {
            get
            {
                return new Uri(@"https://www.pegelonline.wsv.de/webservices/rest-api/v2/");
            }
        }

        public int UpdateFrequency
        {
            get
            {
                return updateFrequency;
            }

            set
            {
                updateFrequency = value;
            }
        }
        public string Configuration
        {
            get
            {
                return configuration;
            }

            set
            {
                configuration = value;
            }
        }
    }
}
