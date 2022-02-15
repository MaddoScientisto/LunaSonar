using LunaSonar.Core;
using MvvmCross.Base;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
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
using System.Configuration;
using LunaSonar.Properties;
using MvvmCross.Binding.BindingContext;
using LunaSonar.Core.ViewModels;

namespace LunaSonar.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MvxWpfView
    {
        public MainView()
        {
            InitializeComponent();

            var set = this.CreateBindingSet<MainView, MainViewModel>();
            set.Bind(this).For(view => view.SaveLoadInteraction).To(viewModel => viewModel.Interaction).OneWay();
            set.Apply();


        }


        #region Settings Save Interaction
        private IMvxInteraction<SettingsInteraction> _saveLoadInteraction;
        public IMvxInteraction<SettingsInteraction> SaveLoadInteraction
        {
            get { return _saveLoadInteraction; }
            set
            {
                if (_saveLoadInteraction != null)
                    _saveLoadInteraction.Requested -= OnSaveInteractionRequested;

                _saveLoadInteraction = value;
                _saveLoadInteraction.Requested += OnSaveInteractionRequested;
            }
        }

        private async void OnSaveInteractionRequested(object sender, MvxValueEventArgs<SettingsInteraction> eventArgs)
        {
            var yesNoQuestion = eventArgs.Value;
            // show dialog
            //var status = await ShowDialog(yesNoQuestion.Question);
            //yesNoQuestion.YesNoCallback(status == DialogStatus.Yes);
            if (yesNoQuestion.OperationType == SettingsOperationType.Save)
            {
                Settings.Default.Username = yesNoQuestion.Settings.UserName;
                Settings.Default.Password = yesNoQuestion.Settings.Password;
                Settings.Default.Token = yesNoQuestion.Settings.Token;
                Settings.Default.Search = yesNoQuestion.Settings.SearchString;
                Settings.Default.Save();
            }
           

            yesNoQuestion.Callback(new Core.Models.SettingsModel
            {
                UserName = Settings.Default.Username,
                Password = Settings.Default.Password,
                Token = Settings.Default.Token,
                SearchString = Settings.Default.Search
            });
        }
        #endregion
    }
}
