using LunaSonar.Core.Models;
using LunaSonar.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaSonar.Console.ServicesImpl
{
    public class ConsoleSettingsService : ISettingsService
    {
        private SettingsModel _settingsModel;
        private readonly ILogger _logger;
        public ConsoleSettingsService(ILogger<ConsoleSettingsService> logger)
        {
            _logger = logger;
        }
        public SettingsModel Load()
        {
            
            try
            {
                var text = File.ReadAllText("config.json");
                _settingsModel = JsonConvert.DeserializeObject<SettingsModel>(text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Demystify(), "Error loading the file");
            }


            return _settingsModel;

        }

        public void Save(SettingsModel model)
        {
            _settingsModel = model;
            var text = JsonConvert.SerializeObject(model);

            try
            {
                File.WriteAllText("config.json", text);
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Demystify(), "Error saving the file");
            }

        }
    }
}
