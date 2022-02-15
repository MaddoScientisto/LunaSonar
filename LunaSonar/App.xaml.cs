using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using LunaSonar.Core;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Serilog;
using MvvmCross.Views;
using MvvmCross.IoC;
using LunaSonar.Core.ViewModels;
using LunaSonar.Views;

namespace LunaSonar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        protected override void RegisterSetup()
        {

            this.RegisterSetupType<Setup>();

        }
    }

    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerProvider CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }

        protected override ILoggerFactory CreateLogFactory()
        {

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            // add more sinks here
            .CreateLogger();

            return new SerilogLoggerFactory();
        }



  
        protected override IMvxViewsContainer? InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup, IMvxIoCProvider iocProvider)
        {
            //viewModelViewLookup.Add(typeof(MainViewModel), typeof(MainView));


            return base.InitializeViewLookup(viewModelViewLookup, iocProvider);
        }
    }
}
