using LunaSonar.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;

namespace LunaSonar.Core.Services
{
    public class VrChatApiService : IVrChatApiService
    {
        private readonly ISettingsService _settingsService;
        private readonly ILogger<VrChatApiService> _logger;
        VRChat.API.Client.Configuration Config;
        public VrChatApiService(ISettingsService settingsService, ILogger<VrChatApiService> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }

        public async Task Authenticate()
        {
            var settings = _settingsService.Load();
            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                Config.Username = settings.UserName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                Config.Password = settings.Password.Trim();
            }

            AuthenticationApi AuthApi = new AuthenticationApi(Config);



            UsersApi UserApi = new UsersApi(Config);
            WorldsApi WorldApi = new WorldsApi(Config);

            try
            {
                // Calling "GetCurrentUser(Async)" logs you in if you are not already logged in.
                CurrentUser CurrentUser = await AuthApi.GetCurrentUserAsync();
                _logger.LogInformation("Logged in as {0}, Current Avatar {1}", CurrentUser.DisplayName, CurrentUser.CurrentAvatar);
                

                var token = await AuthApi.VerifyAuthTokenAsync();

                settings.Token = token.Token;
                _settingsService.Save(settings);
             
            }
            catch (ApiException e)
            {
                _logger.LogError(e.Demystify(), "Exception in the authentication");
              
            }
        }

        public async Task Init()
        {
            Config = new VRChat.API.Client.Configuration();
            var settings = _settingsService.Load();
            if (!string.IsNullOrWhiteSpace(settings.Token))
            {
                //Config.AccessToken = loadedToken;
                SetCookie(settings.Token);
                _logger.LogDebug($"Loaded token: {settings.Token}");                
            }
        }
        private void SetCookie(string cookie)
        {
            var cook = new Cookie("auth", cookie, null, "api.vrchat.cloud");
            ApiClient.CookieContainer.SetCookies(new Uri("https://api.vrchat.cloud"), cook.ToString());
        }

        public async Task<MonitorResult> Monitor()
        {
            var settings = _settingsService.Load();
            MonitorResult monitorResult = new MonitorResult() {
                PresentNames = new List<string>(),
                Successful = false
            };
            
            FriendsApi friendsApi = new FriendsApi(Config);

            try
            {

                var friends = await friendsApi.GetFriendsAsync(offline: false);

                var friendsToCheck = settings.SearchString.Split(',');

                foreach (var friend in friendsToCheck)
                {

                    if (friends.Any(x => x.Status != UserStatus.Offline && x.DisplayName.Equals(friend.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        monitorResult.PresentNames.Add(friend.Trim());
                        _logger.LogInformation($"Trovato {friend.Trim()}");
                    }
                   
                }

                monitorResult.Successful = true;

            }
            catch (ApiException e)
            {
                _logger.LogError(e.Demystify(), "Exception in the Monitoring");
                monitorResult.Error = e.ToStringDemystified();
               
            }
            return monitorResult;
        }
    }
}
