using RocketLeagueGarage.MVVM.View;
using System.Windows;

namespace RocketLeagueGarage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow mainWindow;

        public void ApplicationStart(object sender, StartupEventArgs e)
        {
            mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}