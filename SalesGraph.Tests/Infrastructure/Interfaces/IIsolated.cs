using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesGraph.Tests.Infrastructure.Interfaces
{
    public interface IIsolated
    {
        Task Isolate();
    }
}
