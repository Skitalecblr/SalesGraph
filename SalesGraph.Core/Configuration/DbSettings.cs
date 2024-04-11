using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesGraph.Core.Configuration
{
    public class DbSettings
    {
        public bool UseSeedData { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string Api { get; set; }
    }


}
