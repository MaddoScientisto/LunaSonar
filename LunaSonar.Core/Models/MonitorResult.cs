using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaSonar.Core.Models
{
    public class MonitorResult
    {
        public List<string> PresentNames { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }
}
