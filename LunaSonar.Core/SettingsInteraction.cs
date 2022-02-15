using LunaSonar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaSonar.Core
{
    public class SettingsInteraction
    {        
        public Action<SettingsModel> Callback { get; set; }
        public SettingsOperationType OperationType { get; set; }
        public SettingsModel Settings {get;set;}     
    }

    public enum SettingsOperationType
    {
        Save,
        Load
    }
}
