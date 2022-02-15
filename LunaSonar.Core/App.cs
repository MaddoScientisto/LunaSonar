using LunaSonar.Core.Services;
using LunaSonar.Core.Services.Impl;
using LunaSonar.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;



namespace LunaSonar.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            //Mvx.IoCProvider.RegisterSingleton<IStaffData>(new StaffData());

            //Mvx.IoCProvider.RegisterSingleton<IVrChatApiService>(new VrChatApiService());
            Mvx.IoCProvider.RegisterType<IVrChatApiService, VrChatApiService>();
            Mvx.IoCProvider.RegisterType<ISettingsService, SettingsService>();

            

            RegisterAppStart<MainViewModel>();
        }
    }
}

