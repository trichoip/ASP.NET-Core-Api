using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.ViewModel;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<MVVM>(provider => new MVVM
            {
                DataContext = provider.GetRequiredService<ItemViewModel>()
            });
            services.AddSingleton<ItemViewModel>();
            services.AddSingleton<Canvas>();
            services.AddSingleton<Style>();
            serviceProvider = services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var stp = serviceProvider.GetService<MVVM>();
            stp.Show();
        }
    }
}
