using LunaSonar.Core.Models;
using LunaSonar.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;

namespace LunaSonar.Core.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IVrChatApiService _apiService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<MainViewModel> _logger;

        private SettingsModel _settingsModel;
        VRChat.API.Client.Configuration Config;
        public MainViewModel(IVrChatApiService vrChatApiService, ISettingsService settingsService, ILogger<MainViewModel> logger)
        {
            _apiService = vrChatApiService;
            _settingsService = settingsService;
            _logger = logger;
        }


        private async Task FinishInitializing()
        {
            
            // asincronia???
            _settingsModel = _settingsService.Load();
            _userName = _settingsModel.UserName;
            _password = _settingsModel.Password;
            _searchString = _settingsModel.SearchString;
            _token = _settingsModel.Token;


            Config = new VRChat.API.Client.Configuration();
            var loadedToken = Token;
            if (!string.IsNullOrWhiteSpace(loadedToken))
            {
                //Config.AccessToken = loadedToken;
                SetCookie(loadedToken);
                Debug.WriteLine($"Loaded token: {loadedToken}");
            }
        }
        public override async Task Initialize()
        {
            await base.Initialize();
            // Init here
            //Settings

            var request = new SettingsInteraction
            {
                Callback = async (resultSettings) =>
                {
                   
                    _settingsModel = resultSettings;
                    await FinishInitializing();
                },
                Settings = _settingsModel,
                OperationType = SettingsOperationType.Load
            };
            _interaction.Raise(request);

        }
        private void SetCookie(string cookie)
        {
            var cook = new Cookie("auth", cookie, null, "api.vrchat.cloud");
            ApiClient.CookieContainer.SetCookies(new Uri("https://api.vrchat.cloud"), cook.ToString());
        }

        public ICommand AuthCommand
        {
            get { return new MvxAsyncCommand(Authenticate); }
        }

        public ICommand CheckCommand
        {
            get { return new MvxAsyncCommand(Monitor); }
        }

        private async Task Authenticate()
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Config.Username = UserName.Trim();
                _settingsModel.UserName = UserName.Trim();
            }
            if (!string.IsNullOrWhiteSpace(Password))
            {
                Config.Password = Password.Trim();
               _settingsModel.Password = Password.Trim();
            }

            _settingsService.Save(_settingsModel);

            AuthenticationApi AuthApi = new AuthenticationApi(Config);



            UsersApi UserApi = new UsersApi(Config);
            WorldsApi WorldApi = new WorldsApi(Config);

            try
            {
                // Calling "GetCurrentUser(Async)" logs you in if you are not already logged in.
                CurrentUser CurrentUser = await AuthApi.GetCurrentUserAsync();
                Console.WriteLine("Logged in as {0}, Current Avatar {1}", CurrentUser.DisplayName, CurrentUser.CurrentAvatar);

                var token = await AuthApi.VerifyAuthTokenAsync();

                Token = token.Token;
                _settingsModel.Token = token.Token;
                _settingsService.Save(_settingsModel);
                
                Debug.WriteLine($"Saved token: {token}");

                //User OtherUser = await UserApi.GetUserAsync("usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469");
                //Console.WriteLine("Found user {0}, joined {1}", OtherUser.DisplayName, OtherUser.DateJoined);

                //World World = await WorldApi.GetWorldAsync("wrld_ba913a96-fac4-4048-a062-9aa5db092812");
                //Console.WriteLine("Found world {0}, visits: {1}", World.Name, World.Visits);
                // TODO: Notify
                //MessageBox.Show("Authenticated");
            }
            catch (ApiException e)
            {
                _logger.LogError(e.Demystify(), "Exception in the authentication");

                //Console.WriteLine("Exception when calling API: {0}", e.Message);
                //Console.WriteLine("Status Code: {0}", e.ErrorCode);
                //Console.WriteLine(e.ToString());
                // TODO: Notify
                //MessageBox.Show($"Error {e.ToString}");
            }
        }

        public async Task Monitor()
        {
            _settingsModel.SearchString = SearchString;
            _settingsService.Save(_settingsModel);           

            FriendsApi friendsApi = new FriendsApi(Config);

            try
            {

                var friends = await friendsApi.GetFriendsAsync(offline: false);

                var friendsToCheck = SearchString.Split(',');

                foreach (var friend in friendsToCheck)
                {

                    if (friends.Any(x => x.Status != UserStatus.Offline && x.DisplayName.Equals(friend.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        // TODO Notify
                        //MessageBox.Show($"Utente {friend} Trovato");
                    }
                    else
                    {
                        // TODO Notify
                        //MessageBox.Show($"Utente {friend} non Trovato");
                    }
                }



            }
            catch (ApiException e)
            {
                _logger.LogError(e.Demystify(), "Exception in the Monitoring");
                //Debug.Print("Exception when calling FriendsApi.GetFriends: " + e.Message);
                //Debug.Print("Status Code: " + e.ErrorCode);
                //Debug.Print(e.StackTrace);
                //MessageBox.Show($"Error {e.ToString}");
                //TODO: Notify
            }


        }


        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);

            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);

            }
        }

        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                RaisePropertyChanged(() => SearchString);

            }
        }

        private string _token;
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                RaisePropertyChanged(() => Token);

            }
        }


        //public override async Task Initialize()
        //{
        //    await base.Initialize();

        //}

        private MvxInteraction<SettingsInteraction> _interaction = new MvxInteraction<SettingsInteraction>();

        public MvxInteraction<SettingsInteraction> Interaction => _interaction;
    }

}
