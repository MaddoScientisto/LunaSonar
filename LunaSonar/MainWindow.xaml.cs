using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;
using System.Configuration;
using LunaSonar.Properties;
using System.Net;
using MvvmCross.Platforms.Wpf.Views;

namespace LunaSonar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //var appConf = System.Configuration.ConfigurationManager.AppSettings;
            //txtUserName.Text = Settings.Default.Username;
            //txtPassword.Password = Settings.Default.Password;
            //txtUserToCheck.Text = Settings.Default.Search;
            //txtToken.Text = Settings.Default.Token;

            //Config = new VRChat.API.Client.Configuration();

            //var loadedToken = Settings.Default.Token;
            //if (!string.IsNullOrWhiteSpace(loadedToken))
            //{
            //    //Config.AccessToken = loadedToken;
            //    SetCookie(loadedToken);
            //    Debug.WriteLine($"Loaded token: {loadedToken}");
            //}

            //Config.ApiKey = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26";
        }

        //VRChat.API.Client.Configuration Config;

        //public async Task Authenticate()
        //{
        //    if (!string.IsNullOrWhiteSpace(txtUserName.Text))
        //    {
        //        Config.Username = txtUserName.Text.Trim();
        //        Settings.Default.Username = txtUserName.Text;
        //    }
        //    if (!string.IsNullOrWhiteSpace(txtPassword.Password))
        //    {
        //        Config.Password = txtPassword.Password.Trim();
        //        Settings.Default.Password = txtPassword.Password;
        //    }
            
        //    Settings.Default.Save();

        //    AuthenticationApi AuthApi = new AuthenticationApi(Config);

     

        //    UsersApi UserApi = new UsersApi(Config);
        //    WorldsApi WorldApi = new WorldsApi(Config);

        //    try
        //    {
        //        // Calling "GetCurrentUser(Async)" logs you in if you are not already logged in.
        //        CurrentUser CurrentUser = await AuthApi.GetCurrentUserAsync();
        //        Console.WriteLine("Logged in as {0}, Current Avatar {1}", CurrentUser.DisplayName, CurrentUser.CurrentAvatar);

        //        var token = await AuthApi.VerifyAuthTokenAsync();
        //        txtToken.Text = token.Token;
        //        Settings.Default.Token = token.Token;
        //        Settings.Default.Save();
        //        Debug.WriteLine($"Saved token: {token}");

        //        //User OtherUser = await UserApi.GetUserAsync("usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469");
        //        //Console.WriteLine("Found user {0}, joined {1}", OtherUser.DisplayName, OtherUser.DateJoined);

        //        //World World = await WorldApi.GetWorldAsync("wrld_ba913a96-fac4-4048-a062-9aa5db092812");
        //        //Console.WriteLine("Found world {0}, visits: {1}", World.Name, World.Visits);

        //        MessageBox.Show("Authenticated");
        //    }
        //    catch (ApiException e)
        //    {
        //        Console.WriteLine("Exception when calling API: {0}", e.Message);
        //        Console.WriteLine("Status Code: {0}", e.ErrorCode);
        //        Console.WriteLine(e.ToString());
        //        MessageBox.Show($"Error {e.ToString}");
        //    }

        //}


        //private void SetCookie(string cookie)
        //{
        //    var cook = new Cookie("auth", cookie, null, "api.vrchat.cloud");
        //    ApiClient.CookieContainer.SetCookies(new Uri("https://api.vrchat.cloud"), cook.ToString());
        //}

        //public async Task Monitor()
        //{
        //    Settings.Default.Search = txtUserToCheck.Text;
        //    Settings.Default.Save();

        //    FriendsApi friendsApi = new FriendsApi(Config);

        //    try
        //    {

        //        var friends = await friendsApi.GetFriendsAsync(offline: false);

        //        var friendsToCheck = txtUserToCheck.Text.Split(',');

        //        foreach (var friend in friendsToCheck)
        //        {

        //            if (friends.Any(x => x.Status != UserStatus.Offline && x.DisplayName.Equals(friend.Trim(), StringComparison.OrdinalIgnoreCase)))
        //            {
        //                MessageBox.Show($"Utente {friend} Trovato");
        //            }
        //            else
        //            {
        //                MessageBox.Show($"Utente {friend} non Trovato");
        //            }
        //        }



        //    }
        //    catch (ApiException e)
        //    {
        //        Debug.Print("Exception when calling FriendsApi.GetFriends: " + e.Message);
        //        Debug.Print("Status Code: " + e.ErrorCode);
        //        Debug.Print(e.StackTrace);
        //        MessageBox.Show($"Error {e.ToString}");
        //    }


        //}

        //private async void btnLogin_Click(object sender, RoutedEventArgs e)
        //{
        //    //await Authenticate();
        //}

        //private async void btnCheck_Click(object sender, RoutedEventArgs e)
        //{
        //    //await Monitor();
        //}
    }
}
