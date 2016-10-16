using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace funkyGauge.Models
{
    public interface IAdapter
    {
        string Title { get;}
        string Description { get;}
        Uri Endpoint { get; }
        int UpdateFrequency { get; set; }
        string Configuration { get; set; }
    }
}
