using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WahlDashboard.Models.DataSources
{
    class KeywordQueue : Queue
    {
        public TimeSpan Duration = new TimeSpan(0, 5, 0);
        public override void Enqueue(object o)
        {
            base.Enqueue(o);
            while ((DateTime.Now - (DateTime)Peek()) > Duration)
            {
                Dequeue();
            }
        }
    }
}
