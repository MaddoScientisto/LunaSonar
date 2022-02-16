using LunaSonar.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaSonar.Core.Services
{
    public interface IVrChatApiService
    {
        Task Init();
        Task Authenticate();
        Task<MonitorResult> Monitor();

    }
}
